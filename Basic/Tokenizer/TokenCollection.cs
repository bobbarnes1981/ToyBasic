using System;
using System.Collections.Generic;

namespace Basic.Tokenizer
{
    public class TokenCollection : ITokenCollection
    {
        private int m_offset;

        private List<IToken> m_tokens;

        public TokenCollection()
        {
            m_tokens = new List<IToken>();
        }

        public void Add(IToken token)
        {
            m_tokens.Add(token);
        }

        public bool End
        {
            get { return m_offset >= m_tokens.Count; }
        }

        public IToken Peek()
        {
            return m_tokens[m_offset];
        }

        public IToken Next()
        {
            if (End)
            {
                throw new IndexOutOfRangeException();
            }
            return m_tokens[m_offset++];
        }

        public void Reset()
        {
            m_offset = 0;
        }

        /// <summary>
        /// Discard white space
        /// </summary>
        public void DiscardWhiteSpace()
        {
            while (!End && Peek().TokenType == Tokens.WhiteSpace)
            {
                Next();
            }
        }
    }
}
