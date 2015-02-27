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

        public abstract T Parse(IInterpreter interpreter, ITextStream input);

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

        /// <summary>
        /// Read an expression node; a variable, number or string followed by an optional operator.
        /// Builds a binary tree representing the expression, will return the top node when the end of
        /// the expression is reached.
        /// </summary>
        /// <param name="interpreter"></param>
        /// <param name="input"></param>
        /// <param name="parentNode">The current previous parent node</param>
        /// <returns></returns>
        public INode ReadExpressionNode(IInterpreter interpreter, ITextStream input, INode parentNode)
        {
            // current result node
            INode result = null;

            // read the current expression 
            INode child = ReadExpressionLeaf(interpreter, input);

            // check for an operator
            Operators op = Operators.None;
            input.DiscardSpaces();
            if (!input.End && Operator.VALID_CHARACTERS.Contains(input.Peek()))
            {
                op = ReadOperator(input);
            }

            if (op == Operators.None || op == Operators.BracketRight)
            {
                // no operator found - end of expression
                if (parentNode == null)
                {
                    // end of expression - no previous expression
                    result = child;
                }
                else
                {
                    // end of expression - with previous expression
                    if (parentNode.GetType() == typeof(Operator))
                    {
                        // parent is operator
                        if (parentNode.Right == null)
                        {
                            // parent has no right node
                            parentNode.Right = child;
                            parentNode.Right.Parent = parentNode;

                            result = parentNode;

                            // get top node
                            while (result.Parent != null)
                            {
                                result = result.Parent;
                            }
                        }
                        else
                        {
                            // parent already has a right node - shouldn't happen?
                            throw new ParserError("Parent expression already has a right node");
                        }
                    }
                    else
                    {
                        // parent is leaf - shouldn't happen?
                        throw new ParserError("Parent expression is a leaf node");
                    }
                }
            }
            else
            {
                // operator found - expression continues
                Operator opNode = new Operator(op);
                if (parentNode == null)
                {
                    // more to go - no previous expression
                    opNode.Left = child;
                    opNode.Left.Parent = opNode;

                    result = ReadExpressionNode(interpreter, input, opNode);
                }
                else
                {
                    // more to go - with previous expression
                    if (parentNode.GetType() == typeof(Operator))
                    {
                        // parent is operator
                        if (parentNode.Right == null)
                        {
                            // parent has no right node
                            if ((int)((Operator)parentNode).OperatorType > (int)opNode.OperatorType)
                            {
                                // place above
                                parentNode.Right = child;
                                parentNode.Right.Parent = parentNode;

                                opNode.Left = parentNode;
                                opNode.Left.Parent = opNode;

                                result = ReadExpressionNode(interpreter, input, opNode);
                            }
                            else
                            {
                                // place below
                                opNode.Left = child;
                                opNode.Left.Parent = opNode;

                                parentNode.Right = opNode;
                                parentNode.Right.Parent = parentNode;

                                result = ReadExpressionNode(interpreter, input, opNode);
                            }
                        }
                        else
                        {
                            // parent already has a right node - shouldn't happen?
                            throw new ParserError("Parent expression already has a right node");
                        }
                    }
                    else
                    {
                        // parent is leaf - shouldn't happen?
                        throw new ParserError("Parent expression is a leaf node");
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Read an expression leaf; a string, variable or number
        /// </summary>
        /// <param name="interpreter"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public INode ReadExpressionLeaf(IInterpreter interpreter, ITextStream input)
        {
            preChecks(input, "expression");
            char character = input.Peek();
            INode leafNode;
            switch (character)
            {
                case '"':
                    leafNode = new Value(ReadString(interpreter, input));
                    break;
                case '$':
                    leafNode = new Value(ReadVariable(interpreter, input));
                    break;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    leafNode = new Value(ReadNumber(interpreter, input));
                    break;
                case '(':
                    expect(input, '(');
                    leafNode = new Brackets(ReadExpressionNode(interpreter, input, null));
                    break;
                case '!':
                    expect(input, "!");
                    leafNode = new Not(ReadExpressionNode(interpreter, input, null));
                    break;
                default:
                    throw new ParserError(string.Format("'{0}' is not recognised as the start of a valid expression leaf node", character));
            }

            return leafNode;
        }

        /// <summary>
        /// Read an operator from the input string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Operators ReadOperator(ITextStream input)
        {
            preChecks(input, "operator");
            string operatorString = string.Empty;
            operatorString += input.Next();

            // check next char to see if it is a valid operator
            while (!input.End && Operator.Representations.ContainsValue(operatorString + input.Peek()))
            {
                operatorString += input.Next();
            }

            // if first char was not an operator
            if (!Operator.Representations.ContainsValue(operatorString))
            {
                throw new ParserError(string.Format("'{0}' is not a valid operator", operatorString));
            }

            return Operator.Representations.First(x => x.Value == operatorString).Key;
        }

        /// <summary>
        /// Read a variable; a number of characters prefixed by the variable prefix '$'
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Types.Variable ReadVariable(IInterpreter interpreter, ITextStream input)
        {
            preChecks(input, "variable");
            expect(input, Types.Variable.PREFIX);
            string name = readUntil(input, character => !CHARACTERS.Contains(character));
            INode index = new Value(new Types.Number(-1));
            if (!input.End && input.Peek() == '[')
            {
                expect(input, '[');
                index = ReadExpressionNode(interpreter, input, null);
                expect(input, ']');
            }

            return new Types.Variable(interpreter, name, index);
        }

        /// <summary>
        /// Read an integer; a number of valid numbers
        /// </summary>
        /// <returns></returns>
        public Types.Number ReadNumber(IInterpreter interpreter, ITextStream input)
        {
            preChecks(input, "number");
            int number;
            string numberString = readUntil(input, character => !NUMBERS.Contains(character));
            if (!int.TryParse(numberString, out number))
            {
                throw new ParserError(string.Format("'{0}' is not a number", numberString));
            }

            return new Types.Number(number);
        }

        /// <summary>
        /// Read a string; a number of valid characters surrounded by double quotes
        /// </summary>
        /// <returns></returns>
        public Types.String ReadString(IInterpreter interpreter, ITextStream input)
        {
            preChecks(input, "string");
            expect(input, '\"');
            string output = readUntil(input, character => character == '\"');
            input.Next();
            return new Types.String(output);
        }
    }
}
