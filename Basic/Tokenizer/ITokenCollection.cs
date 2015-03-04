namespace Basic.Tokenizer
{
    public interface ITokenCollection
    {
        void Add(IToken token);
        bool End { get; }
        IToken Peek();
        IToken Next();
        void Reset();
        void DiscardWhiteSpace();
    }
}