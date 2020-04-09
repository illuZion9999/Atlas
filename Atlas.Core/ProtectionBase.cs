namespace Atlas.Core
{
    public abstract class ProtectionBase : StageBase
    {
        protected ProtectionBase(Context ctx) : base(ctx) { }
        public abstract void Execute(ProtectionTargets targets); //TODO: Maybe do separate stages for better extensibility?
    }
}