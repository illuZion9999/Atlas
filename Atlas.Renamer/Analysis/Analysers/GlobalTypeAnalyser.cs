using dnlib.DotNet;

namespace Atlas.Renamer.Analysis.Analysers
{
    //There is no need to rename <Module> aka the global type
    class GlobalTypeAnalyser : IAnalyser
    {
        public void Analyse(IDnlibDef def, RenamerContext ctx)
        {
            if (!(def is TypeDef type)) return;
            
            if (type.IsGlobalModuleType) ctx.Remove(def);
        }
    }
}