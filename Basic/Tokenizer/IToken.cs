namespace Basic.Tokenizer
{
    public interface IToken
    {
        Tokens TokenType { get; }
        string Value { get; }
    }
}
