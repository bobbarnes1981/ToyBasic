namespace Basic.Expressions
{
    /// <summary>
    /// Represents a brackets expression node
    /// </summary>
    public class Brackets : Operator
    {
        private readonly INode m_expression;

        public Brackets(INode expression)
            : base(Operators.BracketLeft)
        {
            m_expression = expression;
        }

        public override INode Left
        {
            get { return m_expression.Left; }

            set { m_expression.Left = value; }
        }

        public override INode Right
        {
            get { return m_expression.Right; }

            set { m_expression.Right = value; }
        }

        public override object Result()
        {
            return m_expression.Result();
        }

        public override string Text
        {
            get { return string.Format("({0})", m_expression.Text); }
        }
    }
}
