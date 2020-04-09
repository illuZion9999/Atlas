using System.Collections.Generic;
using Autofac;
using dnlib.DotNet;

namespace Atlas.Core
{
    public class Context
    {
        protected Context(ILogger logger) => Logger = logger;
        public Context(Project project, ILogger logger) : this(logger) => Project = project;

        public AssemblyDef CurrentAssembly { get; internal set; }
        public ModuleDef CurrentModule { get; internal set; }
        public IList<AssemblyDef> Assemblies { get; } = new List<AssemblyDef>();
        public IList<ModuleDef> Modules { get; } = new List<ModuleDef>();
        public Project Project { get; }
        public ILogger Logger { get; }
        public ILifetimeScope Scope { get; internal set; }
    }
}