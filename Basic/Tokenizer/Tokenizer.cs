using System;
using System.Collections.Generic;
using System.Linq;
using Basic.Errors;

namespace Basic.Tokenizer
{
    public class Tokenizer : ITokenizer
    {
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

        private char VAR_SUFFIX = '$';

        public ITokenCollection Tokenize(ITextStream input)
        {
            ITokenCollection tokens = new TokenCollection();
            while (!input.End)
            {
                tokens.Add(ReadToken(input));
            }

            tokens.Add(new Token(Tokens.EndOfLine, null));

            return tokens;
        }

        private IToken ReadToken(ITextStream input)
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
                            if (input.Position + j >= input.Length || op[j] != input.Peek(j))
                            {
                                match = false;
                                break;
                            }
                        }

                        if (match)
                        {
                            for (int j = 0; j < i; j++)
                            {
                                input.Next();
                            }

                            return new Token(Tokens.Operator, op);
                        }
                    }
                }
            }

            if (LETTERS.Contains(input.Peek()))
            {
                return new Token(Tokens.Text, ReadChars(input, LETTERS));
            }

            if (NUMBERS.Contains(input.Peek()))
            {
                return new Token(Tokens.Number, ReadChars(input, NUMBERS));
            }

            if (WHITE_SPACE.Contains(input.Peek()))
            {
                return new Token(Tokens.WhiteSpace, ReadChars(input, WHITE_SPACE));
            }

            if (BRACKETS.Contains(input.Peek()))
            {
                return new Token(Tokens.Bracket, input.Next().ToString());
            }

            if (INDEXES.Contains(input.Peek()))
            {
                return new Token(Tokens.Index, input.Next().ToString());
            }

            if (DOUBLE_QUOTE == input.Peek())
            {
                string value = string.Empty;
                while(input.Position + 1 < input.Length && input.Peek(1) != DOUBLE_QUOTE)
                {
                    input.Next();
                    value += input.Peek();
                }

                input.Next();
                if (input.Position >= input.Length || input.Peek() != DOUBLE_QUOTE)
                {
                    throw new TokenizerError("Unterminated string token");
                }

                input.Next();
                return new Token(Tokens.String, value);
            }

            if (VAR_SUFFIX == input.Peek())
            {
                return new Token(Tokens.VarSufix, input.Next().ToString());
            }

            throw new Exception();
        }

        private string ReadChars(ITextStream input, IEnumerable<char> validChars)
        {
            string output = string.Empty;
            char next;
            do
            {
                next = input.Peek();
                if (!validChars.Contains(next))
                {
                    break;
                }

                output += input.Next();
            } while (!input.End);

            return output;
        }
    }
}
