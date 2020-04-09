namespace Atlas.Core
{
    public abstract class StageBase
    {
        protected readonly Context _ctx;
        protected readonly ILogger _logger;

        protected StageBase(Context ctx)
        {
            _ctx = ctx;
            _logger = ctx.Logger;
        }

        public abstract string Name { get; }
    }
}