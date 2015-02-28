namespace Basic.Types
{
    public abstract class Type
    {
        protected readonly object m_value;

        protected Type(object value)
        {
            m_value = value;
        }

        public abstract object Value(IInterpreter interpreter);

        public abstract string Text { get; }
    }
}
