﻿using System;
using System.Collections.Generic;
using Basic.Expressions;

namespace Basic.Parsers
{
    public abstract class Parser<T> : IParser<T>
    {
        /// <summary>
        /// Valid numbers
        /// </summary>
        private readonly List<char> NUMBERS = new List<char>
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };

        /// <summary>
        /// Valuid characters
        /// </summary>
        private readonly List<char> CHARACTERS = new List<char>
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        public abstract T Parse(IInterpreter interpreter, ITextStream input);

        /// <summary>
        /// Expect the specified string in the input string
        /// </summary>
        /// <param name="expectedString">Expected string</param>
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
        /// <param name="expectedChar">Expected character</param>
        protected void expect(ITextStream input, char expectedChar)
        {
            preChecks(input, expectedChar.ToString());
            if (input.Peek() != expectedChar)
            {
                throw new Errors.Parser(string.Format("Expecting '{0}' but found '{1}'", expectedChar, input.Peek()));
            }
            input.Next();
        }

        /// <summary>
        /// Read a string; a number of valid characters surrounded by double quotes
        /// </summary>
        /// <returns></returns>
        protected string readString(ITextStream input)
        {
            preChecks(input, "string");
            expect(input, '\"');
            string output = readUntil(input, character => character == '\"');
            input.Next();
            return output;
        }

        /// <summary>
        /// Read an integer; a number of valid numbers
        /// </summary>
        /// <returns></returns>
        protected int readInt(ITextStream input)
        {
            preChecks(input, "int");
            int number;
            string numberString = readUntil(input, character => !NUMBERS.Contains(character));
            if (!int.TryParse(numberString, out number))
            {
                throw new Errors.Parser(string.Format("'{0}' is not a number", numberString));
            }
            return number;
        }

        /// <summary>
        /// Read a variable; a number of characters prefixed by the variable prefix '$'
        /// </summary>
        /// <returns></returns>
        protected string readVariable(ITextStream input)
        {
            preChecks(input, "variable");
            expect(input, Variable.PREFIX);
            return readUntil(input, character => !CHARACTERS.Contains(character));
        }

        /// <summary>
        /// Read characters into a string until <paramref name="untilFunc"/> is true.
        /// </summary>
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
                    throw new Errors.Parser(string.Format("Unexpected end of line"));
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
        /// Check for the end of the input string and discard any white space
        /// </summary>
        /// <param name="expecting"></param>
        protected void preChecks(ITextStream input, string expecting)
        {
            if (input.End)
            {
                throw new Errors.Parser(string.Format("Expecting {0} but reached end of line", expecting));
            }
            input.DiscardSpaces();
        }
    }
}