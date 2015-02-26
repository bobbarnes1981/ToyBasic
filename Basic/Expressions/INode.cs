namespace Basic.Expressions
{
    public interface INode
    {
        INode Parent { get; set; }

        INode Left { get; set; }

        INode Right { get; set; }

        object Result();

        string Text { get; }
    }
}
