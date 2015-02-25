using System;
using System.Collections.Generic;
using System.Linq;
using Basic.Commands;
using Basic.Commands.Program;

namespace Basic
{
    public class Buffer : IBuffer
    {
        private const int START_LINE = 10;
        private const int DEFAULT_STEP = 10;
        private const int DEFAULT_LINE = 0;

        private SortedDictionary<int, ILine> m_buffer;

        private int m_currentLine;

        public Buffer()
        {
            m_buffer = new SortedDictionary<int, ILine>();
        }

        public void Add(ILine line)
        {
            if (line == null)
                throw new ArgumentNullException("line");
            if (line.Number < 1)
                throw new Errors.Buffer(string.Format("Invalid line number '{0}'", line.Number));
            if (line.Command == null)
                throw new Errors.Buffer(string.Format("Invalid line command 'null'"));
            if (line.Command.IsSystem)
                throw new Errors.Buffer(string.Format("System command '{0}' cannot be added to buffer", line.Command.Keyword));
            if (!m_buffer.ContainsKey(line.Number))
            {
                m_buffer.Add(line.Number, null);
            }
            m_buffer[line.Number] = line;
        }

        public void Renumber()
        {
            int newLineNumber = START_LINE;
            SortedDictionary<int, ILine> buffer = new SortedDictionary<int, ILine>();
            // for each line in buffer
            foreach (int oldLineNumber in m_buffer.Keys)
            {
                // get the current line
                ILine line = m_buffer[oldLineNumber];
                // set the new line number
                line.Number = newLineNumber;
                // for all line in buffer
                foreach (int lineNumber in m_buffer.Keys)
                {
                    // get the current line
                    ICommand command = m_buffer[lineNumber].Command;
                    do
                    {
                        // if not system command
                        if (command.IsSystem == false)
                        {
                            switch (command.Keyword)
                            {
                                case Keywords.Goto:
                                    // renumber goto command
                                    Goto cmd = ((Goto) command);
                                    if (cmd.LineNumber == oldLineNumber)
                                    {
                                        cmd.LineNumber = newLineNumber;
                                    }
                                    command = null;
                                    break;
                                case Keywords.If:
                                    // recurse on if statement to check sub-command
                                    command = ((If) command).Command;
                                    break;
                                default:
                                    // no renumber needed
                                    command = null;
                                    break;
                            }
                        }
                        else
                        {
                            // we should not have system commands in the buffer
                            throw new Errors.Buffer(string.Format("System command '{0}' should not be in buffer", command.Keyword));
                        }
                    } while (command != null);
                }
                buffer.Add(newLineNumber, line);

                newLineNumber += DEFAULT_STEP;
            }

            m_buffer = buffer;
        }

        public void Reset()
        {
            m_currentLine = m_buffer.Keys.FirstOrDefault(x => x > DEFAULT_LINE);
        }

        public ILine Fetch
        {
            get
            {
                if (m_currentLine == DEFAULT_LINE)
                {
                    return null;
                }
                int lastLine = m_currentLine;
                m_currentLine = m_buffer.Keys.FirstOrDefault(x => x > m_currentLine);
                return m_buffer[lastLine];
            }
        }

        public int Current
        {
            get { return m_currentLine; }
        }

        public void Jump(int lineNumber)
        {
            m_currentLine = lineNumber;
        }

        public bool End
        {
            get
            {
                return m_currentLine == DEFAULT_LINE;
            }
        }

        public void Clear()
        {
            m_buffer.Clear();
        }
    }
}
