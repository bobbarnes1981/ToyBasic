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

        public override object Result(IInterpreter interpreter)
        {
            return Left.Result(interpreter);
        }

        public override string Text
        {
            get { return string.Format("({0})", Left.Text); }
        }
    }
}
