namespace Basic
{
    public interface IBuffer
    {
        void Add(Line line);
        void Renumber(int step);
        void Reset();
        Line Fetch { get; }
        void Jump(int lineNumber);
        bool End { get; }
        void Clear();
    }
}
