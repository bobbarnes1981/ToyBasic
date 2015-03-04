namespace Basic.Tokenizer
{
    public interface ITextStream
    {
        int Length { get; }

        int Position { get; }

        bool End { get; }

        char Peek();

        char Peek(int offset);

        char Next();

        void DiscardSpaces();
    }
}
