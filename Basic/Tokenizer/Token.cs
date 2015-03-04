namespace Basic.Tokenizer
{
    public class Token : IToken
    {
        public Token(Tokens type, string value)
        {
            TokenType = type;
            Value = value;
        }

        public Tokens TokenType { get; private set; }

        public string Value { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", TokenType, Value);
        }
    }
}
