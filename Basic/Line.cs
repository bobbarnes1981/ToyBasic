using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic
{
    public class Line
    {
        private readonly int m_number;

        private readonly ICommand m_command;

        public Line(int number, ICommand command)
        {
            m_number = number;
            m_command = command;
        }

        public int Number
        {
            get { return m_number; }
        }

        public ICommand Command
        {
            get { return m_command; }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Number, Command.Text);
        }
    }
}
