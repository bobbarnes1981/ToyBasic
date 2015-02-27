namespace Basic.Expressions
{
    /// <summary>
    /// Represents a brackets expression node
    /// </summary>
    public class Brackets : Operator
    {
        public Brackets(INode expression)
            : base(Operators.BracketLeft)
        {
            Left = expression;
        }

        public override object Result()
        {
            return Left.Result();
        }

        public override string Text
        {
            get { return string.Format("({0})", Left.Text); }
        }
    }
}
