﻿namespace Basic.Commands.Program
{
    /// <summary>
    /// Represents the program command 'For'
    /// </summary>
    public class For : Command
    {
        /// <summary>
        /// The variable to modify
        /// </summary>
        private string m_variable;

        /// <summary>
        /// The start value
        /// </summary>
        private int m_start;

        /// <summary>
        /// The end value
        /// </summary>
        private int m_end;

        /// <summary>
        /// The step value
        /// </summary>
        private int m_step;

        /// <summary>
        /// Creates an instance of the <see cref="For"/> class.
        /// </summary>
        /// <param name="variable">the variable to modify</param>
        /// <param name="start">the start value</param>
        /// <param name="end">the end value</param>
        /// <param name="step">the step value</param>
        public For(string variable, int start, int end, int step)
            : base(Keyword.For, false)
        {
            m_variable = variable;
            m_start = start;
            m_end = end;
            m_step = step;
        }

        /// <summary>
        /// Executes the 'For' command by initialising the variable on the heap and adding a frame to the stack provided by the interpreter heap and stack interfaces
        /// </summary>
        /// <param name="interpreter"></param>
        public override void Execute(IInterpreter interpreter)
        {
            IFrame frame = new Frame();
            frame.Set<int>("for_end", m_end);
            frame.Set<int>("for_step", m_step);
            frame.Set<int>("for_line", interpreter.Buffer.Current);
            interpreter.Heap.Set(m_variable, m_start);
            interpreter.Stack.Push(frame);
        }

        /// <summary>
        /// Gets the text representation of the command
        /// </summary>
        public override string Text
        {
            get { return string.Format("{0} {1} = {2} {3} {4} {5} {6}", Keyword.For, m_variable, m_start, Keyword.To, m_end, Keyword.Step, m_step); }
        }
    }
}
