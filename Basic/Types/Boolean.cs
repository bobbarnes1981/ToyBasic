namespace Basic.Types
{
    /// <summary>
    /// Represents a boolean type
    /// </summary>
    public class Boolean : Type
    {
        public Boolean(bool boolean)
            : base(boolean)
        {
        }

        public override object Value(IInterpreter interpreter)
        {
            return (bool)m_value;
        }

        public override string Text
        {
            get { return string.Format("{0}", m_value); }
        }
    }
}
