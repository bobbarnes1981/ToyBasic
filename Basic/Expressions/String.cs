namespace Basic.Expressions
{
    /// <summary>
    /// Represents a string expression node
    /// </summary>
    public class String : Node
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
            get { return string.Format("\"{0}\"", m_text); }
        }
    }
}
