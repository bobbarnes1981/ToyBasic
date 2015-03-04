namespace Basic.Tokenizer
{
    public interface ITokenizer
    {
        ITokenCollection Tokenize(ITextStream input);
    }
}
