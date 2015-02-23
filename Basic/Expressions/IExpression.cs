namespace Basic.Expressions
{
    public interface IExpression
    {
        IExpression Parent { get; set; }

        IExpression Left { get; set; }

        IExpression Right { get; set; }

        object Result();

        string Text { get; }
    }
}
