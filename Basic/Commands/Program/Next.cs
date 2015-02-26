﻿namespace Basic.Commands.Program
{
    /// <summary>
    /// Represents the program command 'Next'
    /// </summary>
    public class Next : Command
    {
        /// <summary>
        /// The variable to modify
        /// </summary>
        private readonly string m_variable;

        /// <summary>
        /// Creates a new instance of the <see cref="Next"/> class.
        /// </summary>
        /// <param name="variable">The variable to modify</param>
        public Next(string variable)
            : base(Keywords.Next, false)
        {
            m_variable = variable;
        }

        public override void Execute(IInterpreter interpreter)
        {
            if (interpreter.Stack.Count == 0
                || !interpreter.Stack.Peek().Exists("for_var")
                || interpreter.Stack.Peek().Get<string>("for_var") != m_variable)
            {
                throw new Errors.Command("Invalid for_var in current stack frame");
            }

            IFrame frame = interpreter.Stack.Pop();
            int value = (int)interpreter.Heap.Get(m_variable);
            if (value < frame.Get<int>("for_end"))
            {
                value += frame.Get<int>("for_step");
                interpreter.Heap.Set(m_variable, value);
                interpreter.Stack.Push(frame);
                interpreter.Buffer.Jump(frame.Get<int>("for_line"));
            }
        }

        public override string Text
        {
            get { return string.Format("{0} {1}{2}", Keywords.Next, Basic.Expressions.Variable.PREFIX, m_variable); }
        }
    }
}
