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
        /// The interpreter interface
        /// </summary>
        private IInterpreter m_interpreter;

        /// <summary>
        /// Input string
        /// </summary>
        private string m_input;

        /// <summary>
        /// Current offset
        /// </summary>
        private int m_offset;

        /// <summary>
        /// Creates a new instance of the <see cref="Parser"/> class.
        /// </summary>
        /// <param name="interpreter">The interpreter interface</param>
        /// <param name="input">The input string to parse</param>
        /// <returns>A parsed line object</returns>
        public Line Parse(IInterpreter interpreter, string input)
        {
            m_interpreter = interpreter;
            m_input = input;
            m_offset = 0;

            int number;

            string numberString = readUntil((offset, data) => offset >= data.Length || data[offset] == ' ');

            if (!int.TryParse(numberString, out number))
            {
                m_offset = 0;
            }

            Commands.ICommand command = readCommand(m_offset == 0);

            return new Line(number, command);
        }

        /// <summary>
        /// Reas a command from the input string
        /// </summary>
        /// <param name="isSystem">True if a system command is expected</param>
        /// <returns>A parsed ICommand object</returns>
        private Commands.ICommand readCommand(bool isSystem)
        {
            Commands.ICommand command;

            string keywordString = readUntil((offset, data) => offset >= data.Length || data[offset] == ' ');
            Keyword keyword;
            if (!Enum.TryParse(keywordString, out keyword))
            {
                throw new Errors.Parser(string.Format("Could not parse keyword {0}", keywordString));
            }

            if (isSystem)
            {
                command = readSystemCommand(keyword);
            }
            else
            {
                command = readProgramCommand(keyword);
            }

            return command;
        }

        /// <summary>
        /// Read a system command from the input string
        /// </summary>
        /// <param name="keyword">Keyword of the expected command</param>
        /// <returns>A parsed ICommand object</returns>
        private Commands.ICommand readSystemCommand(Keyword keyword)
        {
            Commands.ICommand command;
            switch (keyword)
            {
                case Keyword.Exit:
                    expectEnd();
                    command = new Commands.System.Exit();
                    break;
                case Keyword.Run:
                    expectEnd();
                    command = new Commands.System.Run();
                    break;
                case Keyword.List:
                    expectEnd();
                    command = new Commands.System.List();
                    break;
                case Keyword.Clear:
                    expectEnd();
                    command = new Commands.System.Clear();
                    break;
                case Keyword.Renumber:
                    expectEnd();
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
        private Commands.ICommand readProgramCommand(Keyword keyword)
        {
            Commands.ICommand command;
            switch (keyword)
            {
                case Keyword.For:
                    string forVariable = readVariable();
                    discardSpace();
                    expect('=');
                    int start = readInt();
                    expect(Keyword.To);
                    int end = readInt();
                    expect(Keyword.Step);
                    int step = readInt();
                    command = new Commands.Program.For(forVariable, start, end, step);
                    break;
                case Keyword.Next:
                    string nextVariable = readVariable();
                    command = new Commands.Program.Next(nextVariable);
                    break;
                case Keyword.Clear:
                    expectEnd();
                    command = new Commands.Program.Clear();
                    break;
                case Keyword.Goto:
                    command = new Commands.Program.Goto(readInt());
                    break;
                case Keyword.Print:
                    command = new Commands.Program.Print(readExpressionNode(null));
                    expectEnd();
                    break;
                case Keyword.Let:
                    string letVariable = readVariable();
                    discardSpace();
                    expect('=');
                    Expressions.IExpression expression = readExpressionNode(null);
                    command = new Commands.Program.Let(letVariable, expression);
                    break;
                case Keyword.If:
                    Expressions.IExpression comparison = readExpressionNode(null);
                    expect(Keyword.Then);
                    Commands.ICommand subCommand = readCommand(false);
                    command = new Commands.Program.If(comparison, subCommand);
                    break;
                case Keyword.Rem:
                    command = new Commands.Program.Rem(readUntil((offset, data) => offset >= data.Length));
                    break;
                case Keyword.Input:
                    command = new Commands.Program.Input(readVariable());
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
        private string readUntil(Func<int, string, bool> untilFunc)
        {
            string output = string.Empty;
            char currentChar;
            while (true)
            {
                currentChar = m_input[m_offset];
                output += currentChar;
                if (m_offset >= m_input.Length)
                {
                    throw new Errors.Parser(string.Format("Unexpected end of line"));
                }
                m_offset++;
                if (untilFunc(m_offset, m_input))
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
        private void expect(Keyword keyword)
        {
            expect(keyword.ToString());
        }

        /// <summary>
        /// Expect the specified string in the input string
        /// </summary>
        /// <param name="expectedString">Expected string</param>
        private void expect(string expectedString)
        {
            preChecks(expectedString);
            foreach(char expectedChar in expectedString)
            {
                expect(expectedChar);
            }
        }

        /// <summary>
        /// Expect the specified character in the input string
        /// </summary>
        /// <param name="expectedChar">Expected character</param>
        private void expect(char expectedChar)
        {
            preChecks(expectedChar.ToString());
            if (m_input[m_offset] != expectedChar)
            {
                throw new Errors.Parser(string.Format("Expecting '{0}' but found '{1}'", expectedChar, m_input[m_offset]));
            }
            m_offset++;
        }

        /// <summary>
        /// Expect the end of the input string
        /// </summary>
        private void expectEnd()
        {
            if (m_offset < m_input.Length)
            {
                throw new Errors.Parser(string.Format("Expecting end of line but found '{0}'", m_input[m_offset]));
            }
        }

        /// <summary>
        /// Read a string; a number of valid characters surrounded by double quotes
        /// </summary>
        /// <returns></returns>
        private string readString()
        {
            preChecks("string");
            expect('\"');
            string output = readUntil((offset, data) => offset >= data.Length || data[offset] == '\"');
            m_offset++;
            return output;
        }

        /// <summary>
        /// Read an integer; a number of valid numbers
        /// </summary>
        /// <returns></returns>
        private int readInt()
        {
            preChecks("int");
            int number;
            string input = readUntil((offset, data) => offset >= data.Length || !NUMBERS.Contains(data[offset]));
            if (!int.TryParse(input, out number))
            {
                throw new Errors.Parser(string.Format("'{0}' is not a number", input));
            }
            return number;
        }

        /// <summary>
        /// Read a variable; a number of characters prefixed by the variable prefix '$'
        /// </summary>
        /// <returns></returns>
        private string readVariable()
        {
            preChecks("variable");
            expect(VARIABLE_PREFIX);
            return readUntil((offset, data) => offset >= data.Length || !CHARACTERS.Contains(data[offset]));
        }

        /// <summary>
        /// Read an expression node; a variable, number or string followed by an optional operator.
        /// Builds a binary tree representing the expression, will return the top node when the end of
        /// the expression is reached.
        /// </summary>
        /// <param name="parentNode">The current previous parent node</param>
        /// <returns></returns>
        private Expressions.IExpression readExpressionNode(Expressions.IExpression parentNode)
        {
            // current result node
            Expressions.IExpression result = null;

            // read the current expression 
            Expressions.IExpression child = readExpressionLeaf();

            // check for an operator
            OperatorType op = OperatorType.None;
            discardSpace();
            if (m_offset < m_input.Length && OPERATORS.Contains(m_input[m_offset]))
            {
                op = readOperator();
            }
            if (op == OperatorType.None)
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

                    result = readExpressionNode(opNode);
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

                                result = readExpressionNode(opNode);
                            }
                            else
                            {
                                // place below
                                opNode.Left = child;
                                opNode.Left.Parent = opNode;

                                parentNode.Right = opNode;
                                parentNode.Right.Parent = parentNode;

                                result = readExpressionNode(opNode);
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
        private Expressions.IExpression readExpressionLeaf()
        {
            preChecks("expression");
            char character = m_input[m_offset];
            Expressions.IExpression leafNode;
            switch (character)
            {
                case '"':
                    leafNode = new Expressions.String(readString());
                    break;
                case '$':
                    leafNode = new Expressions.Variable(m_interpreter, readVariable());
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
                    leafNode = new Expressions.Number(readInt());
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
        private OperatorType readOperator()
        {
            preChecks("operator");
            OperatorType op;
            string input = readUntil((offset, data) => offset >= data.Length || !OPERATORS.Contains(data[offset]));
            if (!input.TryParseOperator(out op))
            {
                throw new Errors.Parser(string.Format("'{0}' is not a valid operator", input));
            }
            return op;
        }

        /// <summary>
        /// Check for the end of the input string and discard any white space
        /// </summary>
        /// <param name="expecting"></param>
        private void preChecks(string expecting)
        {
            if (m_offset >= m_input.Length)
            {
                throw new Errors.Parser(string.Format("Expecting {0} but reached end of line", expecting));
            }
            discardSpace();
        }

        /// <summary>
        /// Discard white space
        /// </summary>
        private void discardSpace()
        {
            while (m_offset < m_input.Length && m_input[m_offset] == ' ')
            {
                m_offset++;
            }
        }
    }
}
