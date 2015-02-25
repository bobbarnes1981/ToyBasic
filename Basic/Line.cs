﻿using Basic.Commands;

namespace Basic
{
    public class Line : ILine
    {
        private int m_number;

        private readonly ICommand m_command;

        public Line(int number, ICommand command)
        {
            if (number < 1)
                throw new Errors.Line(string.Format("Invalid line number '{0}'", number));
            m_number = number;
            m_command = command;
        }

        public int Number
        {
            get { return m_number; }
            set { m_number = value; }
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
