using Atlas.Core;

namespace Atlas.Protections
{
    public class ExampleProtection : ProtectionBase
    {
        public ExampleProtection(Context ctx) : base(ctx) { }
        public override string Name => "example";
        public override void Execute(ProtectionTargets targets)
        {
            foreach (var def in targets)
            {
                _logger.Info($"Processing {def.Name}");
            }k
        }
    }
}