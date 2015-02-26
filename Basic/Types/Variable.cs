namespace Basic.Types
{
    /// <summary>
    /// Represents a variable type
    /// </summary>
    public class Variable : Type
    {
        private readonly IInterpreter m_interpreter;

        public static readonly char PREFIX = '$';

        public Variable(IInterpreter interpreter, string variable)
            : base(variable)
        {
            m_interpreter = interpreter;
        }

        public override object Value()
        {
            return m_interpreter.Heap.Get(Text);
        }

        public override string Text
        {
            get { return string.Format("{0}{1}", PREFIX, m_value); }
        }
    }
}
