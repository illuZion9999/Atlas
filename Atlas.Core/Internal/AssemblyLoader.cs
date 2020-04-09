using System.IO;
using dnlib.DotNet;

namespace Atlas.Core.Internal
{
    class AssemblyLoader : IAssemblyLoader
    {
        readonly Context _ctx;
        public AssemblyLoader(Context ctx)
        {
            _ctx = ctx;
        }

        public void LoadAssemblies()
        {
            foreach (var assembly in _ctx.Project.Assemblies)
            {
                try
                {
                    var asm = AssemblyDef.Load(Path.Combine(_ctx.Project.BaseDirectory, assembly));
                    _ctx.Assemblies.Add(asm);
                    
                    foreach (var mod in asm.Modules)
                        _ctx.Modules.Add(mod);
                    
                    _ctx.Logger.Success($"Loaded assembly '{asm.Name}'");
                }
                catch
                {
                    _ctx.Logger.Warning($"Failed to load assembly '{assembly}'");
                }
            }
        }
    }
}