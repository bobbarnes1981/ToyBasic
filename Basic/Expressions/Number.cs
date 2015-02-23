namespace Basic.Expressions
{
    public class Number : Expression
    {
        private readonly int m_number;
        
        public Number(int number)
        {
            m_number = number;
        }

        public override object Result()
        {
            return m_number;
        }

        public override string Text
        {
            get { return m_number.ToString(); }
        }
    }
}
