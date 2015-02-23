namespace Basic.Expressions
{
    public abstract class Expression : IExpression
    {
        public IExpression Parent { get; set; }

        public IExpression Left { get; set; }

        public IExpression Right { get; set; }

        public abstract string Text { get; }

        public abstract object Result();
    }
}
