using System;
using System.Collections.Generic;
using System.Linq;
using Atlas.Core.Analysis.Parser;
using Autofac;
using dnlib.DotNet;

namespace Atlas.Core.Analysis
{
    class DeclarativeAnalyser : IDeclarativeAnalyser
    {
        public DeclarativeAnalyser(Context ctx)
        {
            _logger = ctx.Logger;
            _scope = ctx.Scope;
            _mod = ctx.CurrentModule;
            _projSettings = ctx.Project.Settings ?? Array.Empty<Option>();
        }

        readonly ILogger _logger;
        readonly ILifetimeScope _scope;
        readonly ModuleDef _mod;
        readonly IEnumerable<Option> _projSettings;
        readonly IDictionary<IDnlibDef, OptionList> _parsed = new Dictionary<IDnlibDef, OptionList>();

        public IDictionary<IDnlibDef, OptionList> AnalyseAssembly()
        {
            Analyse(_mod.Assembly);
            Analyse(_mod);
            foreach (var type in _mod.Types) 
                AnalyseType(type);

            return _parsed;
        }
        
        void Analyse(IDnlibDef def)
        {
            _parsed[def] = def is AssemblyDef ? new OptionList(_projSettings) : new OptionList(_parsed.Values.First());
            
            //Inherit from containing type if needed...
            if (def is IMemberDef mem && mem.DeclaringType != null && _parsed[mem.DeclaringType].ApplyToMembers)
                _parsed[def].Inherit(_parsed[mem.DeclaringType]);
            
            foreach (var attribute in GetObfuscationAttributes(def))
            {
                if (attribute.Exclude())
                {
                    _parsed[def] = new OptionList()
                    {
                        Exclude = true,
                        ApplyToMembers = attribute.ApplyToMembers()
                    };
                    return;
                }

                try
                {
                    //TODO: Maybe change the syntax to something LISP-like... easier to parse...
                    _parsed[def].Inherit(_scope.Resolve<IAttributeParser>().ParseAttribute(attribute));
                }
                catch (ObfuscationAttributeParserException ex)
                {
                    _logger.Warning($"Ignoring settings for {def}:\n{ex}");
                }
            }
        }

        void AnalyseType(TypeDef type)
        {
            Analyse(type);
            
            foreach (var nest in type.NestedTypes)
                AnalyseType(nest);
            
            foreach (var method in type.Methods)
                Analyse(method);
            
            foreach (var field in type.Fields)
                Analyse(field);
            
            foreach (var property in type.Properties)
                Analyse(property);
            
            foreach (var @event in type.Events)
                Analyse(@event);
        }
        
        static IEnumerable<CustomAttribute> GetObfuscationAttributes(IDnlibDef def)
        {
            for (var i = 0; i < def.CustomAttributes.Count; i++)
            {
                var attribute = def.CustomAttributes[i];
                if (attribute.TypeFullName != "System.Reflection.ObfuscationAttribute")
                    continue;
                
                yield return attribute;
                if (!attribute.StripAfterObfuscation())
                    continue;
                
                def.CustomAttributes.RemoveAt(i);
                i--;
            }
        }
    }
}