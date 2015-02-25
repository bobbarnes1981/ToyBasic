namespace Basic
{
    public interface IBuffer
    {
        void Add(ILine line);
        void Renumber();
        void Reset();
        ILine Fetch { get; }
        int Current { get; }
        void Jump(int lineNumber);
        bool End { get; }
        void Clear();
    }
}
