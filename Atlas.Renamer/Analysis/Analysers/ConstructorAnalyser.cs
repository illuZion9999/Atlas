using dnlib.DotNet;

namespace Atlas.Renamer.Analysis.Analysers
{
    //No need to rename constructors, in a decompiler they will have the type's name
    class ConstructorAnalyser : IAnalyser
    {
        public void Analyse(IDnlibDef def, RenamerContext ctx)
        {
            if (!(def is MethodDef method) || !method.IsConstructor) return;
            
            ctx.Remove(def);
        }
    }
}