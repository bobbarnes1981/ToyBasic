﻿using Basic.Types;

namespace Basic.Commands.Program
{
    /// <summary>
    /// Represents the program command 'Goto'
    /// </summary>
    public class Goto : Command
    {
        /// <summary>
        /// The line number to go to
        /// </summary>
        private Number m_lineNumber;

        /// <summary>
        /// Creates a new instance of the <see cref="Goto"/> class.
        /// </summary>
        /// <param name="lineNumber">The line number to go to</param>
        public Goto(Number lineNumber)
            : base(Keywords.Goto, false)
        {
            m_lineNumber = lineNumber;
        }

        /// <summary>
        /// Executes the 'Goto' command by calling jump on the interpreter buffer interface
        /// </summary>
        /// <param name="interpreter">interpreter to provide the buffer interface</param>
        public override void execute(IInterpreter interpreter)
        {
            interpreter.Buffer.Jump((int)m_lineNumber.Value(interpreter));
        }

        /// <summary>
        /// Gets or sets the line number to jump to
        /// </summary>
        public Number LineNumber
        {
            get { return m_lineNumber; }
            set { m_lineNumber = value; }
        }

        /// <summary>
        /// Gets the text representation of the command
        /// </summary>
        public override string Text
        {
            get { return string.Format("{0} {1}", Keywords.Goto, m_lineNumber.Text); }
        }
    }
}
