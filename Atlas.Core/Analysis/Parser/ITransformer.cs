using System.Collections.Generic;
using Atlas.Core.Analysis.Parser.AST;

namespace Atlas.Core.Analysis.Parser {
    interface ITransformer {
        IList<Option> TransformAST(ASTNode root);
    }
}