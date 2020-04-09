using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Atlas.Core.Analysis;
using Atlas.Core.ReferenceResolving;
using Autofac;

namespace Atlas.Core.Internal
{
    static class Engine
    {
        static ContainerFactory _factory;
        static IContainer _container;

        static IEnumerable<ProtectionBase> _protections;
        
        internal static void Initialise(Project project, ILogger logger)
        {
            logger.Info("Initialising...");
            _factory = new ContainerFactory();
            _factory.RegisterMainTypes();
            _factory.RegisterInstance(logger).As<ILogger>().SingleInstance();
            
            CreateContext(project, logger);
            CreateOutputDirectory(project, logger);
            _container = _factory.Build();
            
            logger.Debug("Loading assemblies...");
            _container.Resolve<IAssemblyLoader>().LoadAssemblies();
            
            logger.Debug("Discovering protections...");
            _protections = _container.Resolve<IPluginDiscovery>().DiscoverProtections();
        }

        internal static void Run()
        {
            var logger = _container.Resolve<ILogger>();
            var ctx = _container.Resolve<Context>();
            var stopwatch = Stopwatch.StartNew();
            var targetsFactory = _container.Resolve<IProtectionTargetsFactory>();
            
            foreach (var asm in ctx.Assemblies)
            {
                ctx.CurrentAssembly = asm;
                foreach (var mod in asm.Modules)
                {
                    logger.Info($"Processing {mod.Name}");
                    ctx.CurrentModule = mod;
                    using var scope = _container.BeginLifetimeScope();
                    ctx.Scope = scope;

                    logger.Info("Resolving references...");
                    _container.Resolve<IResolver>().ResolveReferences();
                        
                    logger.Info("Analysing...");
                    var results = _container.Resolve<IDeclarativeAnalyser>().AnalyseAssembly();
                        
                    foreach (var prot in _protections)
                    {
                        // ReSharper disable once SuspiciousTypeConversion.Global
                        if (prot is INeedsInitialising init)
                            init.Initialise();
                            
                        logger.Info($"Executing stage '{prot.Name}'");
                        prot.Execute(targetsFactory.Build(prot.Name, results));
                    }
                }

                asm.Write(Path.Combine(ctx.Project.OutputDirectory, Path.GetFileName(asm.ManifestModule.Location)));
            }

            stopwatch.Stop();
            logger.Success($"Finished all tasks in {stopwatch.Elapsed}");
        }

        static void CreateContext(Project project, ILogger logger)
        {
            logger.Debug("Creating context...");
            _factory.RegisterInstance(new Context(project, logger)).As<Context>().SingleInstance();
        }

        static void CreateOutputDirectory(Project project, ILogger logger)
        {
            logger.Debug("Creating output directory...");
            Directory.CreateDirectory(project.OutputDirectory);
        }
    }
}