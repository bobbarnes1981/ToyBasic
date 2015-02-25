using System;
using System.Linq;

namespace Basic
{
    /// <summary>
    /// Represents a string parser
    /// </summary>
    public class Parser : IParser
    {
        /// <summary>
        /// The variable prefix character
        /// </summary>
        private const char VARIABLE_PREFIX = '$';

        /// <summary>
        /// Valid numbers
        /// </summary>
        private readonly char[] NUMBERS = 
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };

        /// <summary>
        /// Valid opeerator characters
        /// </summary>
        private readonly char[] OPERATORS = 
        {
            '+', '-', '=', '*', '/', '!'
        };

        /// <summary>
        /// Valuid characters
        /// </summary>
        private readonly char[] CHARACTERS = 
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        /// <summary>
        /// Creates a new instance of the <see cref="Parser"/> class.
        /// </summary>
        /// <param name="input">The input string to parse</param>
        /// <returns>A parsed line object</returns>
        public Line Parse(IInterpreter interpreter, ITextStream input)
        {
            int number;

            string numberString = readUntil(input, character => character == ' ');

            if (!int.TryParse(numberString, out number))
            {
                input.Reset();
            }

            Commands.ICommand command = readCommand(interpreter, input, number == 0);

            return new Line(number, command);
        }

        /// <summary>
        /// Reas a command from the input string
        /// </summary>
        /// <param name="isSystem">True if a system command is expected</param>
        /// <returns>A parsed ICommand object</returns>
        private Commands.ICommand readCommand(IInterpreter interpreter, ITextStream input, bool isSystem)
        {
            Commands.ICommand command;

            Keywords keyword = readKeyword(input);

            if (isSystem)
            {
                command = readSystemCommand(input, keyword);
            }
            else
            {
                command = readProgramCommand(interpreter, input, keyword);
            }

            return command;
        }

        /// <summary>
        /// Read a keyword from the input string
        /// </summary>
        /// <returns></returns>
        private Keywords readKeyword(ITextStream input)
        {
            string keywordString = readUntil(input, character => character == ' ');
            Keywords keyword;
            if (!Enum.TryParse(keywordString, out keyword))
            {
                throw new Errors.Parser(string.Format("Could not parse keyword {0}", keywordString));
            }
            return keyword;
        }

        /// <summary>
        /// Read a system command from the input string
        /// </summary>
        /// <param name="keyword">Keyword of the expected command</param>
        /// <returns>A parsed ICommand object</returns>
        private Commands.ICommand readSystemCommand(ITextStream input, Keywords keyword)
        {
            Commands.ICommand command;
            switch (keyword)
            {
                case Keywords.Clear:
                    expectEnd(input);
                    command = new Commands.System.Clear();
                    break;
                case Keywords.Exit:
                    expectEnd(input);
                    command = new Commands.System.Exit();
                    break;
                case Keywords.List:
                    expectEnd(input);
                    command = new Commands.System.List();
                    break;
                case Keywords.New:
                    expectEnd(input);
                    command = new Commands.System.New();
                    break;
                case Keywords.Run:
                    expectEnd(input);
                    command = new Commands.System.Run();
                    break;
                case Keywords.Renumber:
                    expectEnd(input);
                    command = new Commands.System.Renumber();
                    break;
                default:
                    throw new Errors.Parser(string.Format("System keyword not implemented in line parser: {0}", keyword));
            }
            return command;
        }

        /// <summary>
        /// Read a program command from the input string
        /// </summary>
        /// <param name="keyword">Keyword of the expected command</param>
        /// <returns>A parsed ICommand object</returns>
        private Commands.ICommand readProgramCommand(IInterpreter interpreter, ITextStream input, Keywords keyword)
        {
            Commands.ICommand command;
            switch (keyword)
            {
                case Keywords.Clear:
                    expectEnd(input);
                    command = new Commands.Program.Clear();
                    break;
                case Keywords.Dim:
                    string variable = readVariable(input);
                    // TODO: expect [<number>]
                    throw new NotImplementedException();
                    break;
                case Keywords.For:
                    string forVariable = readVariable(input);
                    discardSpace(input);
                    expect(input, '=');
                    int start = readInt(input);
                    expect(input, Keywords.To);
                    int end = readInt(input);
                    expect(input, Keywords.Step);
                    int step = readInt(input);
                    expectEnd(input);
                    command = new Commands.Program.For(forVariable, start, end, step);
                    break;
                case Keywords.Goto:
                    command = new Commands.Program.Goto(readInt(input));
                    expectEnd(input);
                    break;
                case Keywords.If:
                    Expressions.IExpression comparison = ReadExpressionNode(interpreter, input, null);
                    expect(input, Keywords.Then);
                    command = new Commands.Program.If(comparison, readCommand(interpreter, input, false));
                    expectEnd(input);
                    break;
                case Keywords.Input:
                    command = new Commands.Program.Input(readVariable(input));
                    expectEnd(input);
                    break;
                case Keywords.Let:
                    string letVariable = readVariable(input);
                    discardSpace(input);
                    expect(input, '=');
                    command = new Commands.Program.Let(letVariable, ReadExpressionNode(interpreter, input, null));
                    expectEnd(input);
                    break;
                case Keywords.Next:
                    command = new Commands.Program.Next(readVariable(input));
                    expectEnd(input);
                    break;
                case Keywords.Print:
                    command = new Commands.Program.Print(ReadExpressionNode(interpreter, input, null));
                    expectEnd(input);
                    break;
                case Keywords.Rem:
                    command = new Commands.Program.Rem(readUntil(input, character => false));
                    expectEnd(input);
                    break;
                default:
                    throw new Errors.Parser(string.Format("Program command not implemented in line parser: {0}", keyword));
            }
            return command;
        }

        /// <summary>
        /// Read characters into a string until <paramref name="untilFunc"/> is true.
        /// </summary>
        /// <param name="untilFunc">Function to designate end of string</param>
        /// <returns>A string read from the input string</returns>
        private string readUntil(ITextStream input, Func<char, bool> untilFunc)
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
        /// Expect the spcified keyword in the input string
        /// </summary>
        /// <param name="keyword">Expected keyword</param>
        private void expect(ITextStream input, Keywords keyword)
        {
            expect(input, keyword.ToString());
        }

        /// <summary>
        /// Expect the specified string in the input string
        /// </summary>
        /// <param name="expectedString">Expected string</param>
        private void expect(ITextStream input, string expectedString)
        {
            preChecks(input, expectedString);
            foreach(char expectedChar in expectedString)
            {
                expect(input, expectedChar);
            }
        }

        /// <summary>
        /// Expect the specified character in the input string
        /// </summary>
        /// <param name="expectedChar">Expected character</param>
        private void expect(ITextStream input, char expectedChar)
        {
            preChecks(input, expectedChar.ToString());
            if (input.Peek() != expectedChar)
            {
                throw new Errors.Parser(string.Format("Expecting '{0}' but found '{1}'", expectedChar, input.Peek()));
            }
            input.Next();
        }

        /// <summary>
        /// Expect the end of the input string
        /// </summary>
        private void expectEnd(ITextStream input)
        {
            if (!input.End)
            {
                throw new Errors.Parser(string.Format("Expecting end of line but found '{0}'", input.Peek()));
            }
        }

        /// <summary>
        /// Read a string; a number of valid characters surrounded by double quotes
        /// </summary>
        /// <returns></returns>
        private string readString(ITextStream input)
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
        private int readInt(ITextStream input)
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
        private string readVariable(ITextStream input)
        {
            preChecks(input, "variable");
            expect(input, VARIABLE_PREFIX);
            return readUntil(input, character => !CHARACTERS.Contains(character));
        }

        /// <summary>
        /// Read an expression node; a variable, number or string followed by an optional operator.
        /// Builds a binary tree representing the expression, will return the top node when the end of
        /// the expression is reached.
        /// </summary>
        /// <param name="parentNode">The current previous parent node</param>
        /// <returns></returns>
        public Expressions.IExpression ReadExpressionNode(IInterpreter interpreter, ITextStream input, Expressions.IExpression parentNode)
        {
            // current result node
            Expressions.IExpression result = null;

            // read the current expression 
            Expressions.IExpression child = readExpressionLeaf(interpreter, input);

            // check for an operator
            Operators op = Operators.None;
            discardSpace(input);
            if (!input.End && OPERATORS.Contains(input.Peek()))
            {
                op = readOperator(input);
            }
            if (op == Operators.None)
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
                    if (parentNode.GetType() == typeof (Expressions.Operator))
                    {
                        // parent is operator
                        if (parentNode.Right == null)
                        {
                            // parent has no right node
                            parentNode.Right = child;
                            parentNode.Right.Parent = parentNode;

                            result = parentNode;

                            // get top node
                            while(result.Parent != null)
                            {
                                result = result.Parent;
                            }
                        }
                        else
                        {
                            // parent already has a right node - shouldn't happen?
                            throw new Errors.Parser("Parent expression already has a right node");
                        }
                    }
                    else
                    {
                        // parent is leaf - shouldn't happen?
                        throw new Errors.Parser("Parent expression is a leaf node");
                    }
                }
            }
            else
            {
                // operator found - expression continues
                Expressions.Operator opNode = new Expressions.Operator(op);
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
                    if (parentNode.GetType() == typeof(Expressions.Operator))
                    {
                        // parent is operator
                        if (parentNode.Right == null)
                        {
                            // parent has no right node
                            if (((Expressions.Operator)parentNode).OperatorType > opNode.OperatorType)
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
                            throw new Errors.Parser("Parent expression already has a right node");
                        }
                    }
                    else
                    {
                        // parent is leaf - shouldn't happen?
                        throw new Errors.Parser("Parent expression is a leaf node");
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Read an expression leaf; a string, variable or number
        /// </summary>
        /// <returns></returns>
        private Expressions.IExpression readExpressionLeaf(IInterpreter interpreter, ITextStream input)
        {
            preChecks(input, "expression");
            char character = input.Peek();
            Expressions.IExpression leafNode;
            switch (character)
            {
                case '"':
                    leafNode = new Expressions.String(readString(input));
                    break;
                case '$':
                    leafNode = new Expressions.Variable(interpreter, readVariable(input));
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
                    leafNode = new Expressions.Number(readInt(input));
                    break;
                default:
                    throw new Errors.Parser(string.Format("'{0}' is not recognised as the start of a valid expression leaf node", character));
            }
            return leafNode;
        }

        /// <summary>
        /// Read an operator from the input string
        /// </summary>
        /// <returns></returns>
        private Operators readOperator(ITextStream input)
        {
            preChecks(input, "operator");
            Operators op;
            string operatorString = readUntil(input, character => !OPERATORS.Contains(character));
            if (!operatorString.TryParseOperator(out op))
            {
                throw new Errors.Parser(string.Format("'{0}' is not a valid operator", operatorString));
            }
            return op;
        }

        /// <summary>
        /// Check for the end of the input string and discard any white space
        /// </summary>
        /// <param name="expecting"></param>
        private void preChecks(ITextStream input, string expecting)
        {
            if (input.End)
            {
                throw new Errors.Parser(string.Format("Expecting {0} but reached end of line", expecting));
            }
            discardSpace(input);
        }

        /// <summary>
        /// Discard white space
        /// </summary>
        private void discardSpace(ITextStream input)
        {
            while (!input.End && input.Peek() == ' ')
            {
                input.Next();
            }
        }
    }
}
