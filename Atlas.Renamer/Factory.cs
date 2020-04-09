using Atlas.Core;
using Atlas.Renamer.Analysis;
using Atlas.Renamer.NameProviders;

namespace Atlas.Renamer
{
    static class Factory
    {
        internal static IAnalysePhase CreateAnalysePhase()
        {
            return new AnalysePhase();
        }

        internal static IDefinitionProcessor CreateDefinitionProcessor(RenamerContext ctx, ProtectionTargets targets)
        {
            return new DefinitionProcessor(ctx, targets);
        }

        static readonly INameProvider _ascii = new Ascii();
        internal static INameProvider CreateAscii() => _ascii;
        
        static readonly INameProvider _fileSizeSaving = new FileSizeSaving();
        internal static INameProvider CreateFileSizeSaving() => _fileSizeSaving;
    }
}