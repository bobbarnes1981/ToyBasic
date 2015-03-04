using System.Linq;
using Basic.Commands;
using Basic.Commands.Program;
using Basic.Errors;
using Basic.Expressions;
using Basic.Tokenizer;

namespace Basic.Parser
{
    public class Parser : IParser
    {
        public ILine Parse(ITokenCollection tokens)
        {
            tokens.Reset();

            int number = 0;

            if (tokens.Peek().TokenType == Tokens.Number)
            {
                number = int.Parse(tokens.Next().Value);
            }

            ICommand command = readCommand(tokens, number == 0);

            return new Line(number, command);
        }

        /// <summary>
        /// Reads a command from the token collection
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="isSystem">True if a system command is expected</param>
        /// <returns>A parsed ICommand object</returns>
        private ICommand readCommand(ITokenCollection tokens, bool isSystem)
        {
            ICommand command;

            Keywords keyword = readKeyword(tokens);

            if (isSystem)
            {
                command = readSystemCommand(tokens, keyword);
            }
            else
            {
                command = readProgramCommand(tokens, keyword);
            }

            return command;
        }

        /// <summary>
        /// Read a keyword from the token collection
        /// </summary>
        /// <returns></returns>
        private Keywords readKeyword(ITokenCollection tokens)
        {
            expect(tokens, Tokens.Text);
            string keywordString = tokens.Next().Value;
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
        private ICommand readSystemCommand(ITokenCollection tokens, Keywords keyword)
        {
            ICommand command;
            switch (keyword)
            {
                case Keywords.Clear:
                    expectEnd(tokens);
                    command = new Commands.System.Clear();
                    break;
                case Keywords.Exit:
                    expectEnd(tokens);
                    command = new Commands.System.Exit();
                    break;
                case Keywords.List:
                    expectEnd(tokens);
                    command = new Commands.System.List();
                    break;
                case Keywords.Load:
                    command = new Commands.System.Load(ReadString(tokens));
                    expectEnd(tokens);
                    break;
                case Keywords.New:
                    expectEnd(tokens);
                    command = new Commands.System.New();
                    break;
                case Keywords.Run:
                    expectEnd(tokens);
                    command = new Commands.System.Run();
                    break;
                case Keywords.Renumber:
                    expectEnd(tokens);
                    command = new Commands.System.Renumber();
                    break;
                case Keywords.Save:
                    command = new Commands.System.Save(ReadString(tokens));
                    expectEnd(tokens);
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
        private ICommand readProgramCommand(ITokenCollection tokens, Keywords keyword)
        {
            ICommand command;
            switch (keyword)
            {
                case Keywords.Clear:
                    expectEnd(tokens);
                    command = new Clear();
                    break;
                case Keywords.Dim:
                    Types.Variable variable = ReadVariable(tokens);
                    command = new Dim(variable);
                    break;
                case Keywords.For:
                    Types.Variable forVariable = ReadVariable(tokens);
                    expect(tokens, Tokens.Operator, "=");
                    INode start = ReadExpressionNode(tokens, null);
                    expect(tokens, Tokens.Text, Keywords.To.ToString());
                    INode end = ReadExpressionNode(tokens, null);
                    expect(tokens, Tokens.Text, Keywords.Step.ToString());
                    INode step = ReadExpressionNode(tokens, null);
                    expectEnd(tokens);
                    command = new For(forVariable, start, end, step);
                    break;
                case Keywords.Gosub:
                    command = new Gosub(ReadNumber(tokens));
                    expectEnd(tokens);
                    break;
                case Keywords.Goto:
                    command = new Goto(ReadNumber(tokens));
                    expectEnd(tokens);
                    break;
                case Keywords.If:
                    INode comparison = ReadExpressionNode(tokens, null);
                    expect(tokens, Tokens.Text, Keywords.Then.ToString());
                    command = new If(comparison, readCommand(tokens, false));
                    expectEnd(tokens);
                    break;
                case Keywords.Input:
                    command = new Input(ReadVariable(tokens));
                    expectEnd(tokens);
                    break;
                case Keywords.Let:
                    Types.Variable letVariable = ReadVariable(tokens);
                    expect(tokens, Tokens.Operator, "=");
                    command = new Let(letVariable, ReadExpressionNode(tokens, null));
                    expectEnd(tokens);
                    break;
                case Keywords.Next:
                    command = new Next(ReadVariable(tokens));
                    expectEnd(tokens);
                    break;
                case Keywords.Print:
                    command = new Print(ReadExpressionNode(tokens, null));
                    expectEnd(tokens);
                    break;
                case Keywords.Rem:
                    command = new Rem(readUntil(tokens, Tokens.EndOfLine));
                    expectEnd(tokens);
                    break;
                case Keywords.Return:
                    command = new Return();
                    expectEnd(tokens);
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
        /// <param name="tokens"></param>
        /// <param name="parentNode">The current previous parent node</param>
        /// <returns></returns>
        public INode ReadExpressionNode(ITokenCollection tokens, INode parentNode)
        {
            // current result node
            INode result = null;

            // read the current expression 
            INode child = ReadExpressionLeaf(tokens);

            // check for an operator
            Operators op = Operators.None;
            tokens.DiscardWhiteSpace();
            if (!tokens.End && (tokens.Peek().TokenType == Tokens.Operator || tokens.Peek().TokenType == Tokens.Bracket))
            {
                op = Operator.Representations.First(x => x.Value == tokens.Peek().Value).Key;
                tokens.Next();
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

                    result = ReadExpressionNode(tokens, opNode);
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

                                result = ReadExpressionNode(tokens, opNode);
                            }
                            else
                            {
                                // place below
                                opNode.Left = child;
                                opNode.Left.Parent = opNode;

                                parentNode.Right = opNode;
                                parentNode.Right.Parent = parentNode;

                                result = ReadExpressionNode(tokens, opNode);
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
        /// <param name="tokens"></param>
        /// <returns></returns>
        public INode ReadExpressionLeaf(ITokenCollection tokens)
        {
            preChecks(tokens, "expression");
            INode leafNode;
            switch (tokens.Peek().TokenType)
            {
                case Tokens.String:
                    leafNode = new Value(ReadString(tokens));
                    break;
                case Tokens.Text:
                    leafNode = new Value(ReadVariable(tokens));
                    break;
                case Tokens.Number:
                    leafNode = new Value(ReadNumber(tokens));
                    break;
                case Tokens.Bracket: // check for (
                    tokens.Next();
                    leafNode = new Brackets(ReadExpressionNode(tokens, null));
                    break;
                case Tokens.Operator: // check for !
                    tokens.Next();
                    leafNode = new Not(ReadExpressionNode(tokens, null));
                    break;
                default:
                    throw new ParserError(string.Format("'{0} {1}' is not recognised as the start of a valid expression leaf node", tokens.Peek().TokenType, tokens.Peek().Value));
            }

            return leafNode;
        }

        /// <summary>
        /// Read a variable name
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public Types.Variable ReadVariable(ITokenCollection tokens)
        {
            expect(tokens, Tokens.Text);
            string name = tokens.Next().Value;
            expect(tokens, Tokens.VarSufix);
            tokens.Next();
            INode index = null;
            if (!tokens.End && tokens.Peek().TokenType == Tokens.Index && tokens.Peek().Value == "[")
            {
                expect(tokens, Tokens.Index);
                tokens.Next();
                index = ReadExpressionNode(tokens, null);
                expect(tokens, Tokens.Index);
                tokens.Next();
                // check for ]
            }

            return new Types.Variable(name, index);
        }

        /// <summary>
        /// Read an integer; a number of valid numbers
        /// </summary>
        /// <returns></returns>
        public Types.Number ReadNumber(ITokenCollection tokens)
        {
            expect(tokens, Tokens.Number);
            return new Types.Number(int.Parse(tokens.Next().Value));
        }

        /// <summary>
        /// Read a string; a number of valid characters surrounded by double quotes
        /// </summary>
        /// <returns></returns>
        public Types.String ReadString(ITokenCollection tokens)
        {
            preChecks(tokens, "string");
            expect(tokens, Tokens.String);
            string value = tokens.Next().Value;
            return new Types.String(value);
        }

        /// <summary>
        /// Read tokens into a string until the specified token type is reached.
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="tokenType"></param>
        /// <returns></returns>
        protected string readUntil(ITokenCollection tokens, Tokens tokenType)
        {
            string output = string.Empty;
            IToken currentToken;
            while (true)
            {
                currentToken = tokens.Peek();
                output += currentToken.Value;
                if (tokens.End)
                {
                    throw new ParserError(string.Format("Unexpected end of line"));
                }

                tokens.Next();
                if (tokens.End || tokens.Peek().TokenType == tokenType)
                {
                    break;
                }
            }

            return output;
        }

        protected void expect(ITokenCollection tokens, Tokens tokenType, string value)
        {
            expect(tokens, tokenType);
            if (tokens.Peek().Value != value)
            {
                throw new ParserError(string.Format("Expecting '{0} {1}' but found '{2} {3}'", tokenType, value, tokens.Peek().TokenType, tokens.Peek().Value));
            }

            tokens.Next();
        }

        /// <summary>
        /// Expect the specified tokenType in the token collection
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="tokenType">Expected character</param>
        protected void expect(ITokenCollection tokens, Tokens tokenType)
        {
            preChecks(tokens, tokenType.ToString());
            if (tokens.Peek().TokenType != tokenType)
            {
                throw new ParserError(string.Format("Expecting '{0}' but found '{1}'", tokenType, tokens.Peek().TokenType));
            }
        }

        /// <summary>
        /// Expect the end of the token collection
        /// </summary>
        protected void expectEnd(ITokenCollection tokens)
        {
            if (!tokens.End && tokens.Peek().TokenType != Tokens.EndOfLine)
            {
                throw new ParserError(string.Format("Expecting end of line but found '{0}'", tokens.Peek()));
            }
        }

        /// <summary>
        /// Check for the end of the token collection and discard any white space
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="expecting"></param>
        protected void preChecks(ITokenCollection tokens, string expecting)
        {
            if (tokens.End)
            {
                throw new ParserError(string.Format("Expecting {0} but reached end of line", expecting));
            }

            tokens.DiscardWhiteSpace();
        }
    }
}
