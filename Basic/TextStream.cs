using System;

namespace Basic
{
    public class TextStream : ITextStream
    {
        private string m_text;

        private int m_offset;

        public TextStream(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");
            m_text = text;
            Reset();
        }

        public bool End
        {
            get { return m_offset >= m_text.Length; }
        }

        public char Peek()
        {
            return m_text[m_offset];
        }

        public char Next()
        {
            if (End)
                throw new IndexOutOfRangeException();
            return m_text[m_offset++];
        }

        public void Reset()
        {
            m_offset = 0;
        }

        /// <summary>
        /// Discard white space
        /// </summary>
        public void DiscardSpaces()
        {
            while (!End && Peek() == ' ')
            {
                Next();
            }
        }
    }
}
