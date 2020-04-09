using System.Linq;
using Atlas.Core;
using dnlib.DotNet;

namespace Atlas.Renamer
{
    public class RenamerProtection : ProtectionBase
    {
        public RenamerProtection(Context ctx) : base(ctx) { }
        public override string Name => "rename";
        
        public override void Execute(ProtectionTargets targets)
        {
            var ctx = new RenamerContext
            {
                Logger = _logger
            };
            var phase = Factory.CreateAnalysePhase();

            foreach (var d in targets.Where(d => !(d is AssemblyDef) && !(d is ModuleDef)))
            {
                ctx.Add(d);
            }
            
            foreach (var def in targets)
            {
                phase.Analyse(def, ctx);
            }

            CommenceRename(ctx, targets);
        }

        static void CommenceRename(RenamerContext ctx, ProtectionTargets targets)
        {
            Factory.CreateDefinitionProcessor(ctx, targets).ProcessDefinitions();
        }
    }
}