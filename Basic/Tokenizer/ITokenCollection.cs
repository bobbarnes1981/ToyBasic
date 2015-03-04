namespace Basic.Tokenizer
{
    public interface ITokenCollection
    {
        void Add(IToken token);
        int Length { get; }
        int Position { get; }
        bool End { get; }
        IToken Peek();
        IToken Next();
        void Reset();
        void DiscardWhiteSpace();
    }
}