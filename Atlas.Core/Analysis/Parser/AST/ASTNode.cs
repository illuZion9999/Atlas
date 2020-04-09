using System.Collections.Generic;

namespace Atlas.Core.Analysis.Parser.AST
{
    class ASTNode
    {
        internal string Name { get; set; }
        internal IList<ASTNode> Children { get; } = new List<ASTNode>();
    }
}