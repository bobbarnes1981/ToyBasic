namespace Basic
{
    public interface ILineBuffer
    {
        void Add(ILine line);

        void Renumber(IInterpreter interpreter);

        void Reset();

        bool Next();

        ILine Current { get; }

        void Jump(int lineNumber);

        bool End { get; }

        void Clear();
    }
}
