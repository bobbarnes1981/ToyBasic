using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic
{
    public class Buffer : IBuffer
    {
        private const int DEFAULT_LINE = 0;

        private SortedDictionary<int, Line> m_buffer;

        private int m_currentLine;

        public Buffer()
        {
            m_buffer = new SortedDictionary<int, Line>();
        }

        public void Add(Line line)
        {
            if (!m_buffer.ContainsKey(line.Number))
            {
                m_buffer.Add(line.Number, null);
            }
            m_buffer[line.Number] = line;
        }

        public void Renumber(int step)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            m_currentLine = m_buffer.Keys.FirstOrDefault(x => x > DEFAULT_LINE);
        }

        public Line Fetch
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
