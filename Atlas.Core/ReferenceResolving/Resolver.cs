using System.Collections.Generic;
using System.Linq;
using Atlas.Core.ReferenceResolving.Resolvers;
using dnlib.DotNet;

namespace Atlas.Core.ReferenceResolving
{
    class Resolver : IResolver
    {
        public Resolver(Context ctx)
        {
            _project = ctx.Project;
            _logger = ctx.Logger;
            _module = ctx.CurrentModule;
        }

        readonly Project _project;
        readonly ILogger _logger;
        readonly ModuleDef _module;
        readonly IEnumerable<IReferenceResolver> _resolvers = new IReferenceResolver[]
        {
            new NormalResolver(),
            new CosturaResolver()
        };
        
        public void ResolveReferences()
        {
            var resolver = new AssemblyResolver();
            var moduleContext = new ModuleContext(resolver);
            
            resolver.DefaultModuleContext = moduleContext;
            _module.Context = moduleContext;
            
            foreach (var probe in _project.ExtraProbePaths)
                resolver.PostSearchPaths.Add(probe);

            foreach (var @ref in _module.GetAssemblyRefs())
            {
                if (ResolveRef(@ref, resolver))
                    _logger.Success($"Resolved {@ref.Name}");
                else _logger.Error($"Couldn't resolve {@ref.Name}");
            }
        }

        bool ResolveRef(AssemblyRef reference, AssemblyResolver assemblyResolver)
        {
            return _resolvers.Any(res => res.ResolveReference(reference, _module, assemblyResolver));
        }
    }
}