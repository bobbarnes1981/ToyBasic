namespace Basic.Expressions
{
    public abstract class Node : INode
    {
        public INode Parent { get; set; }

        public virtual INode Left { get; set; }

        public virtual INode Right { get; set; }

        public abstract string Text { get; }

        public abstract object Result(IInterpreter interpreter);
    }
}
