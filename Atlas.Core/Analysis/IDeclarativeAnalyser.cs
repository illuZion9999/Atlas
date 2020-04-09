using System.Collections.Generic;
using dnlib.DotNet;

namespace Atlas.Core.Analysis {
    interface IDeclarativeAnalyser {
        IDictionary<IDnlibDef, OptionList> AnalyseAssembly();
    }
}