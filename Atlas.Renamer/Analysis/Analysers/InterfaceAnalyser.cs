using System.Linq;
using dnlib.DotNet;

namespace Atlas.Renamer.Analysis.Analysers
{
    //We need to match the interface's members' names with the
    //implementing type's members' names
    //or else, it will break...
    class InterfaceAnalyser : IAnalyser
    {
        public void Analyse(IDnlibDef def, RenamerContext ctx)
        {
            if (!(def is TypeDef type) || !type.HasInterfaces) return;
            
            foreach (var inf in type.Interfaces.Select(i => i.Interface))
            {
                var resolved = inf.ResolveTypeDefThrow();
                
                if (resolved.Module != type.Module)
                {
                    foreach (var method in resolved.Methods)
                    {
                        var impl = TryGetImplementation(type, method);
                        if (impl is null) continue;
                        ctx.Remove(impl);
                    }
                    
                    foreach (var prop in resolved.Properties)
                    {
                        var impl = TryGetImplementation(type, prop);
                        if (impl is null) continue;
                        ctx.Remove(impl);
                    }

                    foreach (var ev in resolved.Events)
                    {
                        var impl = TryGetImplementation(type, ev);
                        if (impl is null) continue;
                        ctx.Remove(impl);
                    }
                    
                    continue;
                }
                
                //Link all properties, methods and events that are implemented
                foreach (var method in resolved.Methods)
                {
                    var impl = TryGetImplementation(type, method);
                    if (impl is null) continue;
                    ctx.Link(impl, method);
                }

                foreach (var ev in resolved.Events)
                {
                    var impl = TryGetImplementation(type, ev);
                    if (impl is null) continue;
                    ctx.Link(impl, ev);
                }
                
                //Although technically we don't need to link the properties themselves,
                //only their getter and setter methods
                //but sshhhhhh, let's be good guys and do it anyway
                foreach (var prop in resolved.Properties)
                {
                    var impl = TryGetImplementation(type, prop);
                    if (impl is null) continue;
                    ctx.Link(impl, prop);
                }
            }
        }

        static IMemberDef TryGetImplementation(TypeDef implementingType, IMemberDef interfaceMember)
        {
            var method = SearchMethods(implementingType, interfaceMember);
            if (method != null)
            {
                return method;
            }
            
            var prop = SearchProperties(implementingType, interfaceMember.Name);
            if (prop != null)
            {
                return prop;
            }
            
            var ev = SearchEvents(implementingType, interfaceMember.Name);
            if (ev != null)
            {
                return ev;
            }
            
            if (!implementingType.IsAbstract && !implementingType.IsInterface)
                throw new RenamerAnalyserException($"Couldn't find implementation of '{interfaceMember}' in '{implementingType}'");
            
            return null;
        }

        static IMemberDef SearchMethods(TypeDef implementing, IFullName decl)
        {
            var res = implementing.Methods.SingleOrDefault(m => m.Name == decl.Name);
            if (res != null) return res;

            foreach (var method in implementing.Methods)
            {
                if (!method.HasOverrides) continue;

                foreach (var over in method.Overrides)
                {
                    if (over.MethodDeclaration == decl)
                        return method;
                }
            }

            return null;
        }

        static IMemberDef SearchProperties(TypeDef implementing, string name)
        {
            return implementing.Properties.SingleOrDefault(p => p.Name == name);
        }

        static IMemberDef SearchEvents(TypeDef implementing, string name)
        {
            return implementing.Events.SingleOrDefault(e => e.Name == name);
        }
    }
}