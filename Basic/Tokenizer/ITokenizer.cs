namespace Basic.Tokenizer
{
    public interface ITokenizer
    {
        ITokenCollection Tokenize(string input);
    }
}
