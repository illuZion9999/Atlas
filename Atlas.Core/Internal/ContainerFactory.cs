using Atlas.Core.Analysis;
using Atlas.Core.Analysis.Parser;
using Atlas.Core.Analysis.Parser.AST;
using Atlas.Core.Analysis.Parser.Tokenising;
using Atlas.Core.ReferenceResolving;
using Autofac;

namespace Atlas.Core.Internal
{
    class ContainerFactory : ContainerBuilder
    {
        internal void RegisterMainTypes()
        {
            this.RegisterType<AssemblyLoader>().As<IAssemblyLoader>();
            this.RegisterType<PluginDiscovery>().As<IPluginDiscovery>();
            this.RegisterType<DeclarativeAnalyser>().As<IDeclarativeAnalyser>();
            this.RegisterType<Tokeniser>().As<ITokeniser>();
            this.RegisterType<ASTBuilder>().As<IASTBuilder>();
            this.RegisterType<Transformer>().As<ITransformer>().SingleInstance();
            this.RegisterType<AttributeParser>().As<IAttributeParser>().SingleInstance();
            this.RegisterType<Resolver>().As<IResolver>();
            this.RegisterType<ProtectionTargetsFactory>().As<IProtectionTargetsFactory>().SingleInstance();
        }
    }
}