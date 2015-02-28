using Basic.Commands;
using Basic.Commands.Program;
using Basic.Errors;
using Basic.Expressions;
using Basic.Types;
using System.Linq;

namespace Basic.Parsers
{
    /// <summary>
    /// Represents a line parser
    /// </summary>
    public class LineParser : Parser<ILine>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="LineParser"/> class.
        /// </summary>
        /// <param name="interpreter"></param>
        /// <param name="input">The input string to parse</param>
        /// <returns>A parsed line object</returns>
        public override ILine Parse(ITextStream input)
        {
            int number;

            string numberString = readUntil(input, character => character == ' ');

            if (!int.TryParse(numberString, out number))
            {
                input.Reset();
            }

            ICommand command = readCommand(input, number == 0);

            return new Line(number, command);
        }

        /// <summary>
        /// Reas a command from the input string
        /// </summary>
        /// <param name="interpreter"></param>
        /// <param name="input"></param>
        /// <param name="isSystem">True if a system command is expected</param>
        /// <returns>A parsed ICommand object</returns>
        private ICommand readCommand(ITextStream input, bool isSystem)
        {
            ICommand command;

            Keywords keyword = readKeyword(input);

            if (isSystem)
            {
                command = readSystemCommand(input, keyword);
            }
            else
            {
                command = readProgramCommand(input, keyword);
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
            if (!System.Enum.TryParse(keywordString, out keyword))
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
                    command = new Commands.System.Load(ReadString(input));
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
                    command = new Commands.System.Save(ReadString(input));
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
        private ICommand readProgramCommand(ITextStream input, Keywords keyword)
        {
            ICommand command;
            switch (keyword)
            {
                case Keywords.Clear:
                    expectEnd(input);
                    command = new Clear();
                    break;
                case Keywords.Dim:
                    Types.Variable variable = ReadVariable(input);
                    command = new Dim(variable);
                    break;
                case Keywords.For:
                    Types.Variable forVariable = ReadVariable(input);
                    expect(input, '=');
                    INode start = ReadExpressionNode(input, null);
                    expect(input, Keywords.To);
                    INode end = ReadExpressionNode(input, null);
                    expect(input, Keywords.Step);
                    INode step = ReadExpressionNode(input, null);
                    expectEnd(input);
                    command = new For(forVariable, start, end, step);
                    break;
                case Keywords.Goto:
                    command = new Goto(ReadNumber(input));
                    expectEnd(input);
                    break;
                case Keywords.If:
                    INode comparison = ReadExpressionNode(input, null);
                    expect(input, Keywords.Then);
                    command = new If(comparison, readCommand(input, false));
                    expectEnd(input);
                    break;
                case Keywords.Input:
                    command = new Input(ReadVariable(input));
                    expectEnd(input);
                    break;
                case Keywords.Let:
                    Types.Variable letVariable = ReadVariable(input);
                    expect(input, '=');
                    command = new Let(letVariable, ReadExpressionNode(input, null));
                    expectEnd(input);
                    break;
                case Keywords.Next:
                    command = new Next(ReadVariable(input));
                    expectEnd(input);
                    break;
                case Keywords.Print:
                    command = new Print(ReadExpressionNode(input, null));
                    expectEnd(input);
                    break;
                case Keywords.Rem:
                    command = new Rem(readUntil(input, character => false));
                    expectEnd(input);
                    break;
                default:
                    throw new ParserError(string.Format("Program command not implemented in line parser: {0}", keyword));
            }

            return command;
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
        public INode ReadExpressionNode(ITextStream input, INode parentNode)
        {
            // current result node
            INode result = null;

            // read the current expression 
            INode child = ReadExpressionLeaf(input);

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

                    result = ReadExpressionNode(input, opNode);
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

                                result = ReadExpressionNode(input, opNode);
                            }
                            else
                            {
                                // place below
                                opNode.Left = child;
                                opNode.Left.Parent = opNode;

                                parentNode.Right = opNode;
                                parentNode.Right.Parent = parentNode;

                                result = ReadExpressionNode(input, opNode);
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
        public INode ReadExpressionLeaf(ITextStream input)
        {
            preChecks(input, "expression");
            char character = input.Peek();
            INode leafNode;
            switch (character)
            {
                case '"':
                    leafNode = new Value(ReadString(input));
                    break;
                case '$':
                    leafNode = new Value(ReadVariable(input));
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
                    leafNode = new Value(ReadNumber(input));
                    break;
                case '(':
                    expect(input, '(');
                    leafNode = new Brackets(ReadExpressionNode(input, null));
                    break;
                case '!':
                    expect(input, "!");
                    leafNode = new Not(ReadExpressionNode(input, null));
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
        public Types.Variable ReadVariable(ITextStream input)
        {
            preChecks(input, "variable");
            expect(input, Types.Variable.PREFIX);
            string name = readUntil(input, character => !CHARACTERS.Contains(character));
            INode index = null;
            if (!input.End && input.Peek() == '[')
            {
                expect(input, '[');
                index = ReadExpressionNode(input, null);
                expect(input, ']');
            }

            return new Types.Variable(name, index);
        }

        /// <summary>
        /// Read an integer; a number of valid numbers
        /// </summary>
        /// <returns></returns>
        public Types.Number ReadNumber(ITextStream input)
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
        public Types.String ReadString(ITextStream input)
        {
            preChecks(input, "string");
            expect(input, '\"');
            string output = readUntil(input, character => character == '\"');
            input.Next();
            return new Types.String(output);
        }
    }
}
