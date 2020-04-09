using System;
using System.Collections.Generic;
using Atlas.Core.Analysis.Parser.AST;
using Atlas.Core.Analysis.Parser.Tokenising;
using Autofac;
using dnlib.DotNet;

namespace Atlas.Core.Analysis.Parser
{
    class AttributeParser : IAttributeParser
    {
        public AttributeParser(Context ctx)
        {
            _scope = ctx.Scope;
        }

        readonly ILifetimeScope _scope;
        
        public OptionList ParseAttribute(CustomAttribute attribute)
        {
            return new OptionList(Parse(attribute.Feature()))
            {
                ApplyToMembers = attribute.ApplyToMembers()
            };
        }

        IList<Option> Parse(string feature)
        {
            if (string.IsNullOrEmpty(feature))
                return Array.Empty<Option>();
            
            var tokens = _scope.Resolve<ITokeniser>().Tokenise(feature);
            var ast = _scope.Resolve<IASTBuilder>().Parse(tokens);
            return _scope.Resolve<ITransformer>().TransformAST(ast);
        }
    }
}