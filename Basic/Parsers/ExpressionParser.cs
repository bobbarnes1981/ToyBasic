using System.Linq;
using Basic.Errors;
using Basic.Expressions;

namespace Basic.Parsers
{
    /// <summary>
    /// Represents an expression parser
    /// </summary>
    public class ExpressionParser : Parser<INode>
    {
        public override INode Parse(IInterpreter interpreter, ITextStream input)
        {
            return ReadExpressionNode(interpreter, input, null);
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
                    leafNode = new Value(readString(input));
                    break;
                case '$':
                    leafNode = new Value(readVariable(interpreter, input));
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
                    leafNode = new Value(readNumber(input));
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
            while (Operator.Representations.ContainsValue(operatorString + input.Peek()))
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
    }
}
