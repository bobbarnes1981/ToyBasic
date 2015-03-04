using System;
using System.Collections.Generic;
using System.Linq;

namespace Basic.Tokenizer
{
    public class Tokenizer : ITokenizer
    {
        private int m_offset;
        private string m_input;

        private char[] LETTERS =
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        private char[] NUMBERS =
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };

        private string[] OPERATORS =
        {
            "<=", ">=","||", "&&", "==", "!=", "<", ">", "+", "-", "/", "*", "%", "&", "|", "=", "!"
        };

        private char[] WHITE_SPACE =
        {
            ' ', '\r', '\n', '\t'
        };

        private char DOUBLE_QUOTE = '"';

        private char[] BRACKETS =
        {
            '(', ')'
        };

        private char[] INDEXES =
        {
            '[', ']'
        };

        private char VAR_SUFIX = '$';

        public ITokenCollection Tokenize(string input)
        {
            m_offset = 0;
            m_input = input;
            ITokenCollection tokens = new TokenCollection();
            while (m_offset < input.Length)
            {
                tokens.Add(ReadToken());
            }

            tokens.Add(new Token(Tokens.EOL, null));

            return tokens;
        }

        private IToken ReadToken()
        {
            // match the first longest in the list of operators
            int max = OPERATORS.ToList().Max(x => x.Length);
            for (int i = max; i > 0; i--)
            {
                foreach (string op in OPERATORS)
                {
                    if (op.Length == i)
                    {
                        bool match = true;
                        for (int j = 0; j < i; j++)
                        {
                            if (m_offset + j >= m_input.Length || op[j] != m_input[m_offset + j])
                            {
                                match = false;
                                break;
                            }
                        }

                        if (match)
                        {
                            m_offset += i;
                            return new Token(Tokens.Operator, op);
                        }
                    }
                }
            }

            if (LETTERS.Contains(m_input[m_offset]))
            {
                return new Token(Tokens.Text, ReadTill((prev, next) => !LETTERS.Contains(next)));
            }

            if (NUMBERS.Contains(m_input[m_offset]))
            {
                return new Token(Tokens.Number, ReadTill((prev, next) => !NUMBERS.Contains(next)));
            }

            if (WHITE_SPACE.Contains(m_input[m_offset]))
            {
                return new Token(Tokens.WhiteSpace, ReadTill((prev, next) => !WHITE_SPACE.Contains(next)));
            }

            if (BRACKETS.Contains(m_input[m_offset]))
            {
                return new Token(Tokens.Bracket, m_input[m_offset++].ToString());
            }

            if (INDEXES.Contains(m_input[m_offset]))
            {
                return new Token(Tokens.Index, m_input[m_offset++].ToString());
            }

            if (DOUBLE_QUOTE == m_input[m_offset])
            {
                return new Token(Tokens.Quote, m_input[m_offset++].ToString());
            }

            if (VAR_SUFIX == m_input[m_offset])
            {
                return new Token(Tokens.VarSufix, m_input[m_offset++].ToString());
            }

            throw new Exception();
        }

        private string ReadTill(Func<char, char, bool> endFunc)
        {
            string output = string.Empty;
            char prev = ' ';
            char next;
            do
            {
                next = m_input[m_offset];
                if (endFunc(prev, next))
                {
                    break;
                }

                m_offset++;
                output += next;
                prev = next;
            } while (m_offset < m_input.Length);

            return output;
        }
    }
}
