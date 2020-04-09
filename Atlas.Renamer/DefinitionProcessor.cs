using System.Collections.Generic;
using Atlas.Core;
using Atlas.Renamer.NameProviders;
using dnlib.DotNet;
using Microsoft.CSharp.RuntimeBinder;
using Rivers;

namespace Atlas.Renamer
{
    class DefinitionProcessor : IDefinitionProcessor
    {
        readonly RenamerContext _ctx;
        readonly ProtectionTargets _targets;

        internal DefinitionProcessor(RenamerContext ctx, ProtectionTargets targets)
        {
            _ctx = ctx;
            _targets = targets;
        }

        readonly ISet<IMDTokenProvider> _processed = new HashSet<IMDTokenProvider>();
        
        public void ProcessDefinitions()
        {
            foreach (var root in _ctx.Graph.GetRootNodes())
            {
                dynamic def = root.UserData["def"];
                var renamer = (INameProvider)_ctx.GetRenamer(def, _targets);
                
                ProcessNode(root, renamer.GenerateName(def));
            }
        }

        void ProcessNode(Node node, string name)
        {
            foreach (var dep in _ctx.Graph.GetDerivedNodes(node))
            {
                ProcessNode(dep, name);
            }
            
            if (!(bool)node.UserData["rename"]) return;
            RenameDef(node.UserData["def"], name);
        }

        void RenameDef(dynamic def, string name)
        {
            if (_processed.Contains(def)) return;
            
            def.Name = name;

            try
            {
                def.Namespace = string.Empty;
            }
            catch (RuntimeBinderException)
            {
                //We can't erase the namespace for some things like EmbeddedResource...
            }
            
            if (def is MethodDef method)
                RenameMethodExtra(method);

            _processed.Add(def);
        }

        void RenameMethodExtra(MethodDef method)
        {
            var provider = _ctx.GetRenamer(method, _targets);
            
            foreach (var p in method.Parameters)
            {
                p.Name = provider.GenerateName(p);
            }

            if (!method.HasBody) return;

            foreach (var local in method.Body.Variables)
            {
                local.Name = provider.GenerateName(local);
            }
        }
    }
}