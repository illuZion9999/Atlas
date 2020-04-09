using dnlib.DotNet;

namespace Atlas.Renamer.Analysis.Analysers
{
    class PublicAnalyser : IAnalyser
    {
        public void Analyse(IDnlibDef def, RenamerContext ctx)
        {
            if (!(def is IMemberDef member) || def is PropertyDef || def is EventDef) return;
            
            dynamic real = member; //There is no interface that would allow us to `member.IsPublic` :/
            if (real.IsPublic)
            {
                ctx.Remove(def);
            }
        }
    }
}