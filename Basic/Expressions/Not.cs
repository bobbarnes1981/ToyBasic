using Basic.Errors;

namespace Basic.Expressions
{
    /// <summary>
    /// Represents a not expression node
    /// </summary>
    public class Not : Operator
    {
        public Not(INode expression)
            : base(Operators.Not)
        {
            Left = expression;
        }

        public override object Result()
        {
            object value = Left.Result();
            if (value.GetType() != typeof (bool))
            {
                throw new ExpressionError("Not expects type to be Boolean");
            }

            return !(bool) value;
        }

        public override string Text
        {
            get { return string.Format("{0}{1}", Representations[Operators.Not], Left.Text); }
        }
    }
}
