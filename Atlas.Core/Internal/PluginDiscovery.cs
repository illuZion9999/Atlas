using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Atlas.Core.Internal
{
    class PluginDiscovery : IPluginDiscovery
    {
        readonly Context _ctx;
        public PluginDiscovery(Context ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<ProtectionBase> DiscoverProtections()
        {
            var list = new List<ProtectionBase>();
            var plugins = new List<string>
            {
                "Atlas.Protections.dll",
                "Atlas.Renamer.dll"
            };
            
            if (_ctx.Project.Plugins != null)
                plugins.AddRange(_ctx.Project.Plugins);
            
            foreach (var path in plugins)
            {
                try
                {
                    var asm = Assembly.LoadFile(Path.GetFullPath(path));
                    foreach (var type in asm.ExportedTypes)
                    {
                        if (!typeof(ProtectionBase).IsAssignableFrom(type))
                            continue;
                        
                        list.Add((ProtectionBase)Activator.CreateInstance(type, _ctx));
                    }
                    
                    _ctx.Logger.Success($"Loaded plugin '{asm.GetName().Name}'");
                }
                catch
                {
                    _ctx.Logger.Warning($"Failed to load plugin '{path}'");
                }
            }
            
            return list;
        }
    }
}