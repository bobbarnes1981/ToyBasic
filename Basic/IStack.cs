namespace Basic
{
    public interface IStack
    {
        void Push(IFrame frame);

        IFrame Pop();

        IFrame Peek();

        int Count { get; }
    }
}
