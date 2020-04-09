using System.Collections.Generic;
using Atlas.Core.Analysis.Parser.Tokenising;

namespace Atlas.Core.Analysis.Parser.AST {
    interface IASTBuilder {
        ASTNode Parse(IList<Token> tokens);
    }
}