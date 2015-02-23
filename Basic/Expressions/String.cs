namespace Basic.Expressions
{
    public class String : Expression
    {
        private readonly string m_text;

        public String(string text)
        {
            m_text = text;
        }

        public override object Result()
        {
            return m_text;
        }

        public override string Text
        {
            get { return m_text; }
        }
    }
}
