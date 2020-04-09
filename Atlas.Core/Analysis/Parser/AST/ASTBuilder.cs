using System;
using System.Collections.Generic;
using System.Linq;
using Atlas.Core.Analysis.Parser.AST.Nodes;
using Atlas.Core.Analysis.Parser.Tokenising;

namespace Atlas.Core.Analysis.Parser.AST
{
    class ASTBuilder : IASTBuilder
    {
        IList<Token> _tokens;
        ASTNode _root;
        int _current;
        
        public ASTNode Parse(IList<Token> tokens)
        {
            _tokens = tokens;
            _root = new ASTNode
            {
                Name = "Main"
            };

            while (_current < _tokens.Count)
            {
                var node = Walk();
                switch (node) {
                    case null:
                        continue;
                    case LiteralExpression _:
                        _root.Children.Add(new ProtectionExpression
                        {
                            Name = node.Name
                        });
                        continue;
                    default:
                        _root.Children.Add(node);
                        break;
                }
            }

            return _root;
        }


        ASTNode Walk()
        {
            var token = _tokens[_current++];

            switch (token.Type)
            {
                case TokenTypes.Plus:
                    return new ProtectionExpression
                    {
                        Name = Walk().Name
                    };
                case TokenTypes.Minus:
                    return new ProtectionExpression
                    {
                        Apply = false,
                        Name = Walk().Name
                    };
                case TokenTypes.Parenthesis:
                    if (token.Value[0] == Tokens.LeftRoundbracket)
                    {
                        var last = _root.Children.LastOrDefault();
                        if (!(last is ProtectionExpression)) throw new ObfuscationAttributeParserException("Expected protection name before '('");
                        
                        while (_tokens[_current].Type != TokenTypes.Parenthesis && _tokens[_current].Value[0] != Tokens.RightRoundbracket)
                        {
                            if (_tokens[_current].Type == TokenTypes.Comma)
                            {
                                _current++;
                                continue;
                            }
                            var name = Walk();
                            var colon = Walk();
                            if (!(colon is ColonExpression)) throw new ObfuscationAttributeParserException("Expected ':'");
                            var param = Walk();
                            
                            last.Children.Add(new ProtectionParameterExpression
                            {
                                Name = name.Name,
                                Value = param.Name
                            });
                        }
                    }
                    break;
                case TokenTypes.Colon:
                    return new ColonExpression();
                case TokenTypes.Literal:
                    return new LiteralExpression
                    {
                        Name = token.Value
                    };
                case TokenTypes.Comma:
                    break;
                default:
                    throw new ObfuscationAttributeParserException($"Unexpected token: '{token.Value}'");
            }

            return null;
        }
    }
}