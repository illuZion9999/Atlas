using dnlib.DotNet;

namespace Atlas.Renamer.Analysis
{
    interface IAnalyser
    {
        void Analyse(IDnlibDef def, RenamerContext ctx);
    }
}