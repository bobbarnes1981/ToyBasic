using System;
using System.Linq;

namespace Basic
{
    public class Parser : IParser
    {
        private const char VARIABLE_PREFIX = '$';
        private readonly char[] NUMBERS = 
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };
        private readonly char[] OPERATORS = 
        {
            '+', '-', '=', '*', '/', '!'
        };
        private readonly char[] CHARACTERS = 
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };
        private IInterpreter m_interpreter;
        private string m_input;
        private int m_offset;

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

        private void expect(Keyword keyword)
        {
            expect(keyword.ToString());
        }

        private void expect(string expectedString)
        {
            preChecks(expectedString);
            foreach(char expectedChar in expectedString)
            {
                expect(expectedChar);
            }
        }

        private void expect(char expectedChar)
        {
            preChecks(expectedChar.ToString());
            if (m_input[m_offset] != expectedChar)
            {
                throw new Errors.Parser(string.Format("Expecting '{0}' but found '{1}'", expectedChar, m_input[m_offset]));
            }
            m_offset++;
        }

        private void expectEnd()
        {
            if (m_offset < m_input.Length)
            {
                throw new Errors.Parser(string.Format("Expecting end of line but found '{0}'", m_input[m_offset]));
            }
        }

        private string readString()
        {
            preChecks("string");
            expect('\"');
            string output = readUntil((offset, data) => offset >= data.Length || data[offset] == '\"');
            m_offset++;
            return output;
        }

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

        private string readVariable()
        {
            preChecks("variable");
            expect(VARIABLE_PREFIX);
            return readUntil((offset, data) => offset >= data.Length || !CHARACTERS.Contains(data[offset]));
        }

        private Expressions.IExpression readExpressionNode(Expressions.IExpression parentNode)
        {
            Expressions.IExpression result = null;

            Expressions.IExpression child = readExpressionLeaf();

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
                            throw new NotImplementedException();
                        }
                    }
                    else
                    {
                        // parent is leaf - shouldn't happen?
                        throw new NotImplementedException();
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
                            throw new NotImplementedException();
                        }
                    }
                    else
                    {
                        // parent is leaf - shouldn't happen?
                        throw new NotImplementedException();
                    }
                }
            }

            return result;
        }

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
                    throw new Errors.Parser(string.Format("'{0}' is not recognised as the start of a valid expression", character));
            }
            return leafNode;
        }

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

        private void preChecks(string expecting)
        {
            if (m_offset >= m_input.Length)
            {
                throw new Errors.Parser(string.Format("Expecting {0} but reached end of line", expecting));
            }
            discardSpace();
        }

        private void discardSpace()
        {
            while (m_offset < m_input.Length && m_input[m_offset] == ' ')
            {
                m_offset++;
            }
        }
    }
}
