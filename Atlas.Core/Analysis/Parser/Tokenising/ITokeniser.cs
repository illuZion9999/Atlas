using System.Collections.Generic;

namespace Atlas.Core.Analysis.Parser.Tokenising {
    interface ITokeniser {
        IList<Token> Tokenise(string input);
    }
}