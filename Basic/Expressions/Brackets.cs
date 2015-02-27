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
            // maybe store expression and return it here (instead of using Left)?
            return Left.Result();
        }

        public override string Text
        {
            get { return string.Format("({0})", Left.Text); }
        }
    }
}
