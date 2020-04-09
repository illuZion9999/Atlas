using System.Collections.Generic;

namespace Atlas.Core.Analysis.Parser.Tokenising
{
    class Tokeniser : ITokeniser
    {
        readonly IList<Token> _tokens = new List<Token>();

        public IList<Token> Tokenise(string input)
        {
            var current = 0;
            while (current < input.Length)
            {
                var c = input[current];

                switch (c)
                {
                    case Tokens.Plus:
                        _tokens.Add(new Token(TokenTypes.Plus, c));
                        break;
                    case Tokens.Minus:
                        _tokens.Add(new Token(TokenTypes.Minus, c));
                        break;
                    case Tokens.LeftRoundbracket:
                        _tokens.Add(new Token(TokenTypes.Parenthesis, c));
                        break;
                    case Tokens.RightRoundbracket:
                        _tokens.Add(new Token(TokenTypes.Parenthesis, c));
                        break;
                    case Tokens.Colon:
                        _tokens.Add(new Token(TokenTypes.Colon, c));
                        break;
                    case Tokens.Comma:
                        _tokens.Add(new Token(TokenTypes.Comma, c));
                        break;
                    case Tokens.Whitespace:
                        AppendWhitespace(c);
                        break;
                    default:
                        if (!IsPossibleLiteralCharacter(c))
                            throw new ObfuscationAttributeParserException($"Unexpected token at position {current + 1}: '{c}'");
                        
                        AddLiteral(c);
                        break;
                }
                
                current++;
            }

            return _tokens;
        }

        void AppendWhitespace(char c)
        {
            if (_tokens.Count > 0 && _tokens[_tokens.Count - 1].Type == TokenTypes.Literal)
            {
                _tokens[_tokens.Count - 1].Value += c; 
            }
        }

        void AddLiteral(char c)
        {
            if (_tokens.Count < 1 || _tokens[_tokens.Count - 1].Type != TokenTypes.Literal)
            {
                var token = new Token(TokenTypes.Literal, c);
                _tokens.Add(token);
            }
            else _tokens[_tokens.Count - 1].Value += c;
        }
        
        static bool IsPossibleLiteralCharacter(char c)
        {
            return c >= 'A' && c <= 'z' || c >= '0' && c <= '9';
        }
    }
}