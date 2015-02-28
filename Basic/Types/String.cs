namespace Basic.Types
{
    /// <summary>
    /// Represents a string type
    /// </summary>
    public class String : Type
    {
        public String(string text)
            : base(text)
        {
        }

        public override object Value(IInterpreter interpreter)
        {
            return (string)m_value;
        }

        public override string Text
        {
            get { return string.Format("\"{0}\"", m_value); }
        }
    }
}
