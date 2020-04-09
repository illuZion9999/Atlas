using System.Collections.Generic;
using dnlib.DotNet;

namespace Atlas.Renamer.Analysis
{
    static class HelperExtensions
    {
        internal static void AcceptAnalysers(this IDnlibDef def, RenamerContext ctx, IEnumerable<IAnalyser> analysers)
        {
            foreach (var analyser in analysers)
                analyser.Analyse(def, ctx);
        }
    }
}