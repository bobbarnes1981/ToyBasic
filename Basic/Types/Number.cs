namespace Basic.Types
{
    /// <summary>
    /// Represents a number type
    /// </summary>
    public class Number : Type
    {
        public Number(int number)
            : base(number)
        {
        }

        public override object Value()
        {
            return (int)m_value;
        }

        public override string Text
        {
            get { return m_value.ToString(); }
        }
    }
}
