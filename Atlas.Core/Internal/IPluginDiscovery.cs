using System.Collections.Generic;

namespace Atlas.Core.Internal {
    interface IPluginDiscovery {
        IEnumerable<ProtectionBase> DiscoverProtections();
    }
}