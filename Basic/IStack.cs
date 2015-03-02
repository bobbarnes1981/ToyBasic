namespace Basic
{
    public interface IStack
    {
        IFrame Push();

        IFrame Pop();

        IFrame Peek();

        int Count { get; }
    }
}
