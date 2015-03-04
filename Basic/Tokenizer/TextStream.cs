using System;

namespace Basic.Tokenizer
{
    public class TextStream : ITextStream
    {
        private string m_text;

        private int m_position;

        public TextStream(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            m_text = text;
            m_position = 0;
        }

        public int Length
        {
            get { return m_text.Length;}
        }

        public int Position
        {
            get { return m_position; }
        }

        public bool End
        {
            get { return m_position >= m_text.Length; }
        }

        public char Peek(int offset)
        {
            return m_text[m_position + offset];
        }

        public char Peek()
        {
            return m_text[m_position];
        }

        public char Next()
        {
            if (End)
            {
                throw new IndexOutOfRangeException();
            }
            return m_text[m_position++];
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
