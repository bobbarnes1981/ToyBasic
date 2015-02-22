using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Errors;
using Basic.Expressions;

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
        private string m_input;
        private int m_offset;

        public Line Parse(string input)
        {
            m_input = input;
            m_offset = 0;

            int number;

            string numberString = readUntil((offset, data) => offset >= data.Length || data[offset] == ' ');

            if (!int.TryParse(numberString, out number))
            {
                m_offset = 0;
            }

            ICommand command = readCommand();

            return new Line(number, command);
        }

        private ICommand readCommand()
        {
            ICommand command;

            string operationString = readUntil((offset, data) => offset >= data.Length || data[offset] == ' ');
            Operation operation;
            if (!Enum.TryParse(operationString, out operation))
            {
                throw new ParserError(string.Format("Could not parse command {0}", operationString));
            }

            switch (operation)
            {
                case Operation.Exit:
                    expectEnd();
                    command = new Basic.Commands.Exit();
                    break;
                case Operation.Run:
                    expectEnd();
                    command = new Basic.Commands.Run();
                    break;
                case Operation.List:
                    expectEnd();
                    command = new Basic.Commands.List();
                    break;
                case Operation.Clear:
                    expectEnd();
                    command = new Basic.Commands.Clear();
                    break;
                case Operation.Renumber:
                    expectEnd();
                    command = new Basic.Commands.Renumber();
                    break;
                case Operation.Goto:
                    command = new Basic.Commands.Goto(readInt());
                    break;
                case Operation.Print:
                    command = new Basic.Commands.Print(readExpression());
                    expectEnd();
                    break;
                case Operation.Let:
                    string variable = readVariable();
                    discardSpace();
                    expect('=');
                    IExpression expression = readExpression();
                    command = new Basic.Commands.Let(variable, expression);
                    break;
                case Operation.If:
                    IExpression comparison = readExpression();
                    expect("THEN");
                    ICommand subCommand = readCommand();
                    command = new Basic.Commands.If(comparison, subCommand);
                    break;
                case Operation.Rem:
                    command = new Basic.Commands.Rem(readUntil((offset, data) => offset >= data.Length));
                    break;
                default:
                    throw new InterpreterError(string.Format("Operation not implemented in line parser: {0}", operation));
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
                    throw new ParserError(string.Format("Unexpected end of line"));
                }
                m_offset++;
                if (untilFunc(m_offset, m_input))
                {
                    break;
                }
            }
            return output;
        }

        private void expect(string expectedString)
        {
            if (m_offset >= m_input.Length)
            {
                throw new ParserError(string.Format("Expecting '{0}' but reached end of line", expectedString));
            }
            discardSpace();
            foreach(char expectedChar in expectedString)
            {
                expect(expectedChar);
            }
        }

        private void expect(char expectedChar)
        {
            if (m_offset >= m_input.Length)
            {
                throw new ParserError(string.Format("Expecting '{0}' but reached end of line", expectedChar));
            }
            discardSpace();
            if (m_input[m_offset] != expectedChar)
            {
                throw new ParserError(string.Format("Expecting '{0}' but found '{1}'", expectedChar, m_input[m_offset]));
            }
            m_offset++;
        }

        private void expectEnd()
        {
            discardSpace();
            if (m_offset < m_input.Length)
            {
                throw new ParserError(string.Format("Expecting end of line but found '{0}'", m_input[m_offset]));
            }
        }

        private string readString()
        {
            expect('\"');
            string output = readUntil((offset, data) => offset >= data.Length || data[offset] == '\"');
            m_offset++;
            return output;
        }

        private int readInt()
        {
            // TODO: pre checks
            int number;
            string input = readUntil((offset, data) => offset >= data.Length || !NUMBERS.Contains(data[offset]));
            if (!int.TryParse(input, out number))
            {
                throw new ParserError(string.Format("'{0}' is not a number", input));
            }
            return number;
        }

        private string readVariable()
        {
            if (m_offset >= m_input.Length)
            {
                throw new ParserError("Expecting variable but reached end of line");
            }
            discardSpace();
            expect(VARIABLE_PREFIX);
            return readUntil((offset, data) => offset >= data.Length || !CHARACTERS.Contains(data[offset]));
        }

        private IExpression readExpression()
        {
            if (m_offset >= m_input.Length)
            {
                throw new ParserError("Expecting expression but reached end of line");
            }
            discardSpace();
            char character = m_input[m_offset];
            IExpression expression;
            switch(character)
            {
                case '"':
                    expression = new StringConstant(readString());
                    break;
                case '$':
                    expression = new Variable(readVariable());
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
                    expression = new Number(readInt());
                    break;
                default:
                    throw new ParserError(string.Format("'{0}' is not recognised as the start of a valid expression", character));
            }
            discardSpace();
            if (m_offset < m_input.Length && OPERATORS.Contains(m_input[m_offset]))
            {
                expression.Child(readOperator(), readExpression());
            }
            return expression;
        }

        private Operator readOperator()
        {
            if (m_offset >= m_input.Length)
            {
                throw new ParserError("Expecting operator but reached end of line");
            }
            discardSpace();
            Operator op;
            string input = readUntil((offset, data) => offset >= data.Length || !OPERATORS.Contains(data[offset]));
            if (!input.TryParseOperator(out op))
            {
                throw new ParserError(string.Format("'{0}' is not a valid operator", input));
            }
            return op;
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
