using System;

namespace Atlas.Core.Analysis.Parser
{
    public class ObfuscationAttributeParserException : Exception
    {
        public ObfuscationAttributeParserException(string message) : base(message) {}
    }
}