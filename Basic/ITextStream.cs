namespace Basic
{
    public interface ITextStream
    {
        bool End { get; }
        char Peek();
        char Next();
        void Reset();
    }
}
