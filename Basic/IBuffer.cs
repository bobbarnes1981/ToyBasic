namespace Basic
{
    public interface IBuffer
    {
        void Add(Line line);
        void Renumber(int step);
        void Reset();
        Line Fetch { get; }
        int Current { get; }
        void Jump(int lineNumber);
        bool End { get; }
        void Clear();
    }
}
