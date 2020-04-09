using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Atlas.Core.ReferenceResolving.Resolvers
{
    class CosturaResolver : IReferenceResolver
    {
        IList<Instruction> _costuraCtor;
        bool _checked;
        
        void ProcessModule(ModuleDef module)
        {
            if (_checked) return;
            _checked = true;
            
            var assemblyLoader = module.Types.FirstOrDefault(t => t.Name == "AssemblyLoader" && t.Namespace == "Costura");
            if (assemblyLoader is null) return;

            _costuraCtor = assemblyLoader.FindStaticConstructor().Body.Instructions;
            FindEmbeddedAssemblies();
            ExtractAssemblies(module);
        }

        readonly IList<string> _embeddedAssemblyNames = new List<string>();

        void FindEmbeddedAssemblies()
        {
            for (var i = 1; i < _costuraCtor.Count; i++)
            {
                var curr = _costuraCtor[i];
                if (curr.OpCode != OpCodes.Ldstr || _costuraCtor[i - 1].OpCode != OpCodes.Ldstr)
                    continue;

                var resName = ((string)curr.Operand).ToLowerInvariant();
                if (resName.EndsWith(".pdb") || resName.EndsWith(".pdb.compressed"))
                {
                    i++;
                    continue;
                }

                _embeddedAssemblyNames.Add((string)curr.Operand);
            }
        }

        static byte[] Decompress(Stream dataStream)
        {
            using (var def = new DeflateStream(dataStream, CompressionMode.Decompress))
            {
                var ms = new MemoryStream();
                def.CopyTo(ms);
                ms.Position = 0;
                return ms.ToArray();
            }
        }

        readonly IDictionary<string, AssemblyDef> _cache = new Dictionary<string, AssemblyDef>();
        
        void ExtractAssemblies(ModuleDef module)
        {
            foreach (var res in _embeddedAssemblyNames)
            {
                if (res.StartsWith("costura.costura.dll")) continue;

                var compressed = res.EndsWith(".compressed");
                var raw = module.Resources.FindEmbeddedResource(res);
                var data = compressed ? Decompress(raw.CreateReader().AsStream()) : raw.CreateReader().ToArray();
                var asm = AssemblyDef.Load(data);
                _cache[asm.Name] = asm;
            }
        }
        
        public bool ResolveReference(AssemblyRef reference, ModuleDef module, AssemblyResolver resolver)
        {
            ProcessModule(module);
            if (_cache.Count < 1) return false;

            var found = _cache.TryGetValue(reference.Name, out var asm);
            if (!found) return false;
            
            resolver.AddToCache(asm);
            resolver.ResolveThrow(reference, module);
            return true;

        }
    }
}