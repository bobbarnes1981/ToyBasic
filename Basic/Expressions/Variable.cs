namespace Basic.Expressions
{
    /// <summary>
    /// Represents a variable expression node
    /// </summary>
    public class Variable : Node
    {
        private readonly IInterpreter m_interpreter;

        private readonly string m_variable;

        public static readonly char PREFIX = '$';

        public Variable(IInterpreter interpreter, string variable)
        {
            m_interpreter = interpreter;
            m_variable = variable;
        }

        public override object Result()
        {
            return m_interpreter.Heap.Get(m_variable);
        }

        public override string Text
        {
            get { return m_variable; }
        }
    }
}
