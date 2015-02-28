using Basic.Types;
using System.Collections.Generic;

namespace Basic.Commands.System
{
    /// <summary>
    /// Represents the system command 'Save'
    /// </summary>
    public class Save : Command
    {
        private readonly String m_filename;

        /// <summary>
        /// Creates a new instance of the <see cref="Save"/> class.
        /// </summary>
        public Save(String filename)
            : base(Keywords.Save, true)
        {
            m_filename = filename;
        }

        /// <summary>
        /// Executes the 'Save' command by creating a list of all the lines and sending them to the storage interface save method
        /// </summary>
        /// <param name="interpreter"></param>
        public override void execute(IInterpreter interpreter)
        {
            List<string> lines = new List<string>();
            interpreter.Buffer.Reset();
            while (!interpreter.Buffer.End)
            {
                interpreter.Buffer.Next();
                lines.Add(interpreter.Buffer.Current.ToString());
            }

            interpreter.Storage.Save((string)m_filename.Value(interpreter), lines);
        }

        /// <summary>
        /// Gets the text representation of the command
        /// </summary>
        public override string Text
        {
            get { return string.Format("{0} {1}", Keywords.Save, m_filename.Text); }
        }
    }
}
