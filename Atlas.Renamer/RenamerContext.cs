using Atlas.Core;
using Atlas.Renamer.NameProviders;
using dnlib.DotNet;
using ILogger = Atlas.Core.ILogger;

namespace Atlas.Renamer
{
    class RenamerContext
    {
        internal NameGraph Graph { get; } = new NameGraph();
        
        internal ILogger Logger { get; set; }
        
        internal void Add(IMDTokenProvider def)
        {
            Graph.Add(def);
        }

        internal void Remove(IMDTokenProvider def)
        {
            Graph.Remove(def);
        }
        
        internal void Link(IMDTokenProvider from, IMDTokenProvider to)
        {
            Graph.Link(from, to);
        }

        internal INameProvider GetRenamer(IMDTokenProvider def, ProtectionTargets targets)
        {
            return targets.GetOption(def, "mode", "ascii").ToUpperInvariant() switch
            {
                "FILESIZESAVING" => Factory.CreateFileSizeSaving(),
                _ => Factory.CreateAscii()
            };
        }
    }
}