using System;
using System.Linq;
using Basic.Errors;
using Basic.Expressions;

namespace Basic.Parsers
{
    public abstract class Parser<T> : IParser<T>
    {
        /// <summary>
        /// Valid numbers
        /// </summary>
        protected readonly char[] NUMBERS =
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };

        /// <summary>
        /// Valuid characters
        /// </summary>
        protected readonly char[] CHARACTERS =
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        public abstract T Parse(ITextStream input);

        /// <summary>
        /// Read characters into a string until <paramref name="untilFunc"/> is true.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="untilFunc">Function to designate end of string</param>
        /// <returns>A string read from the input string</returns>
        protected string readUntil(ITextStream input, Func<char, bool> untilFunc)
        {
            string output = string.Empty;
            char currentChar;
            while (true)
            {
                currentChar = input.Peek();
                output += currentChar;
                if (input.End)
                {
                    throw new ParserError(string.Format("Unexpected end of line"));
                }

                input.Next();
                if (input.End || untilFunc(input.Peek()))
                {
                    break;
                }
            }

            return output;
        }

        /// <summary>
        /// Expect the spcified keyword in the input string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="keyword">Expected keyword</param>
        protected void expect(ITextStream input, Keywords keyword)
        {
            expect(input, keyword.ToString());
        }

        /// <summary>
        /// Expect the specified string in the input string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="string">Expected string</param>
        protected void expect(ITextStream input, string expectedString)
        {
            preChecks(input, expectedString);
            foreach (char expectedChar in expectedString)
            {
                expect(input, expectedChar);
            }
        }

        /// <summary>
        /// Expect the specified character in the input string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="expectedChar">Expected character</param>
        protected void expect(ITextStream input, char expectedChar)
        {
            preChecks(input, expectedChar.ToString());
            if (input.Peek() != expectedChar)
            {
                throw new ParserError(string.Format("Expecting '{0}' but found '{1}'", expectedChar, input.Peek()));
            }

            input.Next();
        }

        /// <summary>
        /// Expect the end of the input string
        /// </summary>
        protected void expectEnd(ITextStream input)
        {
            if (!input.End)
            {
                throw new ParserError(string.Format("Expecting end of line but found '{0}'", input.Peek()));
            }
        }

        /// <summary>
        /// Check for the end of the input string and discard any white space
        /// </summary>
        /// <param name="input"></param>
        /// <param name="expecting"></param>
        protected void preChecks(ITextStream input, string expecting)
        {
            if (input.End)
            {
                throw new ParserError(string.Format("Expecting {0} but reached end of line", expecting));
            }

            input.DiscardSpaces();
        }
    }
}
