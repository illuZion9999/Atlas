using dnlib.DotNet;

namespace Atlas.Renamer.Analysis
{
    interface IAnalysePhase
    {
        void Analyse(IDnlibDef def, RenamerContext ctx);
    }
}