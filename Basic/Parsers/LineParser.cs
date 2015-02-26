using System;
using Basic.Commands;
using Basic.Errors;
using Basic.Expressions;

namespace Basic.Parsers
{
    /// <summary>
    /// Represents a line parser
    /// </summary>
    public class LineParser : Parser<ILine>
    {
        private IParser<INode> m_expressionParser;

        public LineParser(IParser<INode> expressionParser)
        {
            m_expressionParser = expressionParser;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="LineParser"/> class.
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

            ICommand command = readCommand(interpreter, input, number == 0);

            return new Line(number, command);
        }

        /// <summary>
        /// Reas a command from the input string
        /// </summary>
        /// <param name="interpreter"></param>
        /// <param name="input"></param>
        /// <param name="isSystem">True if a system command is expected</param>
        /// <returns>A parsed ICommand object</returns>
        private ICommand readCommand(IInterpreter interpreter, ITextStream input, bool isSystem)
        {
            ICommand command;

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
            preChecks(input, "keyword");
            string keywordString = readUntil(input, character => character == ' ');
            Keywords keyword;
            if (!Enum.TryParse(keywordString, out keyword))
            {
                throw new ParserError(string.Format("Could not parse keyword {0}", keywordString));
            }

            return keyword;
        }

        /// <summary>
        /// Read a system command from the input string
        /// </summary>
        /// <param name="keyword">Keyword of the expected command</param>
        /// <returns>A parsed ICommand object</returns>
        private ICommand readSystemCommand(ITextStream input, Keywords keyword)
        {
            ICommand command;
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
                case Keywords.Load:
                    command = new Commands.System.Load(readString(input));
                    expectEnd(input);
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
                case Keywords.Save:
                    command = new Commands.System.Save(readString(input));
                    expectEnd(input);
                    break;
                default:
                    throw new ParserError(string.Format("System keyword not implemented in line parser: {0}", keyword));
            }

            return command;
        }

        /// <summary>
        /// Read a program command from the input string
        /// </summary>
        /// <param name="keyword">Keyword of the expected command</param>
        /// <returns>A parsed ICommand object</returns>
        private ICommand readProgramCommand(IInterpreter interpreter, ITextStream input, Keywords keyword)
        {
            ICommand command;
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
                    INode comparison = m_expressionParser.Parse(interpreter, input);
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
                    throw new ParserError(string.Format("Program command not implemented in line parser: {0}", keyword));
            }

            return command;
        }

        /// <summary>
        /// Expect the spcified keyword in the input string
        /// </summary>
        /// <param name="input"></param>
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
                throw new ParserError(string.Format("Expecting end of line but found '{0}'", input.Peek()));
            }
        }
    }
}
