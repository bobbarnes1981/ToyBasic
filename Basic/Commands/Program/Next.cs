using Basic.Errors;
using Basic.Expressions;
using Basic.Types;

namespace Basic.Commands.Program
{
    /// <summary>
    /// Represents the program command 'Next'
    /// </summary>
    public class Next : Command
    {
        /// <summary>
        /// The variable to modify
        /// </summary>
        private readonly Variable m_variable;

        /// <summary>
        /// Creates a new instance of the <see cref="Next"/> class.
        /// </summary>
        /// <param name="variable">The variable to modify</param>
        public Next(Variable variable)
            : base(Keywords.Next, false)
        {
            m_variable = variable;
        }

        public override void execute(IInterpreter interpreter)
        {
            if (interpreter.Stack.Count == 0
                || !interpreter.Stack.Peek().Exists("for_var")
                || interpreter.Stack.Peek().Get<string>("for_var") != m_variable.Name)
            {
                throw new CommandError("Invalid for_var in current stack frame");
            }

            IFrame frame = interpreter.Stack.Pop();
            int value = (int)interpreter.Heap.Get(m_variable.Name);
            if (value < frame.Get<int>("for_end"))
            {
                value += frame.Get<int>("for_step");
                m_variable.Set(interpreter, value);
                interpreter.Stack.Push(frame);
                interpreter.Buffer.Jump(frame.Get<int>("for_line"));
            }
        }

        public override string Text
        {
            get { return string.Format("{0} {1}", Keywords.Next, m_variable.Text); }
        }
    }
}
