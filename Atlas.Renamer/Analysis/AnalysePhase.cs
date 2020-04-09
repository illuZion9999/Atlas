using System.Collections.Generic;
using Atlas.Renamer.Analysis.Analysers;
using dnlib.DotNet;

namespace Atlas.Renamer.Analysis
{
    class AnalysePhase : IAnalysePhase
    {
        readonly IEnumerable<IAnalyser> _analysers = new IAnalyser[]
        {
            new PublicAnalyser(),
            new ConstructorAnalyser(),
            new GlobalTypeAnalyser(),
            new InterfaceAnalyser(),
            new WinFormsAnalyser(), 
            new WpfAnalyser()
        };

        public void Analyse(IDnlibDef def, RenamerContext ctx)
        {
            if (def is AssemblyDef || def is ModuleDef) return;
            def.AcceptAnalysers(ctx, _analysers);
        }
    }
}