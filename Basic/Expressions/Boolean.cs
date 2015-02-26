namespace Basic.Expressions
{
    /// <summary>
    /// Represents a boolean expression node
    /// </summary>
    public class Boolean : Node
    {
        private readonly bool m_boolean;

        public Boolean(bool boolean)
        {
            m_boolean = boolean;
        }

        public override object Result()
        {
            return m_boolean;
        }

        public override string Text
        {
            get { return string.Format("{0}", m_boolean); }
        }
    }
}
