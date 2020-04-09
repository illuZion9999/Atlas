using System.Collections.Generic;
using Atlas.Core.Analysis.Parser.AST;
using Atlas.Core.Analysis.Parser.AST.Nodes;

namespace Atlas.Core.Analysis.Parser
{
    class Transformer : ITransformer
    {
        public IList<Option> TransformAST(ASTNode root)
        {
            var list = new List<Option>();
            foreach (var prot in root.Children)
            {
                var expr = prot as ProtectionExpression;
                if (list.Exists(o => o.Name == expr.Name))
                    throw new ObfuscationAttributeParserException($"Expression with name '{expr.Name}' already exists");
                    
                var obj = new Option
                {
                    Apply = expr.Apply,
                    Name = expr.Name
                };

                foreach (var child in expr.Children)
                {
                    var param = child as ProtectionParameterExpression;
                    obj.Parameters[param.Name] = param.Value;
                }
                
                list.Add(obj);
            }

            return list;
        }
    }
}