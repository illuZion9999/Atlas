namespace Atlas.Core.Analysis.Parser.Tokenising
{
    class Token
    {
        internal Token(TokenTypes type, string value)
        {
            Type = type;
            Value = value;
        }

        internal Token(TokenTypes type, char value)
        {
            Type = type;
            Value = value.ToString();
        }
        
        internal TokenTypes Type { get; }
        internal string Value { get; set; }
    }
}