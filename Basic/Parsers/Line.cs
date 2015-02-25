using System;
using Basic.Expressions;

namespace Basic.Parsers
{
    /// <summary>
    /// Represents a line parser
    /// </summary>
    public class Line : Parser<ILine>
    {
        private IParser<IExpression> m_expressionParser;

        public Line(IParser<IExpression> expressionParser)
        {
            m_expressionParser = expressionParser;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Line"/> class.
        /// </summary>
        /// <param name="interpreter"></param>
        /// <param name="input">The input string to parse</param>
        /// <returns>A parsed line object</returns>
        public override ILine Parse(IInterpreter interpreter, ITextStream input)
        {
            int number;

            string numberString = readUntil(input, character => character == ' ');

            if (!int.TryParse(numberString, out number))
            {
                input.Reset();
            }

            Commands.ICommand command = readCommand(interpreter, input, number == 0);

            return new Basic.Line(number, command);
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
                    input.DiscardSpaces();
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
                    IExpression comparison = m_expressionParser.Parse(interpreter, input);
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
                    input.DiscardSpaces();
                    expect(input, '=');
                    command = new Commands.Program.Let(letVariable, m_expressionParser.Parse(interpreter, input));
                    expectEnd(input);
                    break;
                case Keywords.Next:
                    command = new Commands.Program.Next(readVariable(input));
                    expectEnd(input);
                    break;
                case Keywords.Print:
                    command = new Commands.Program.Print(m_expressionParser.Parse(interpreter, input));
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
        /// Expect the spcified keyword in the input string
        /// </summary>
        /// <param name="keyword">Expected keyword</param>
        private void expect(ITextStream input, Keywords keyword)
        {
            expect(input, keyword.ToString());
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
    }
}
