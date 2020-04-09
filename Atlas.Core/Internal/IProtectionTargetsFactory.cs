using System.Collections.Generic;
using Atlas.Core.Analysis;
using dnlib.DotNet;

namespace Atlas.Core.Internal {
    interface IProtectionTargetsFactory {
        ProtectionTargets Build(string protName, IDictionary<IDnlibDef, OptionList> dict);
    }
}