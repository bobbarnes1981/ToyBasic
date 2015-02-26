using System.Collections.Generic;
using Basic.Errors;

namespace Basic.Expressions
{
    /// <summary>
    /// Represents an operator expression node
    /// </summary>
    public class Operator : Node
    {
        /// <summary>
        /// Operator to use
        /// </summary>
        private readonly Operators m_operatorType;

        /// <summary>
        /// Valid operator characters
        /// </summary>
        public static readonly List<char> VALID_CHARACTERS = new List<char>
        {
            '+', '-', '=', '*', '/', '!', '&', '|', '^', ';', '<', '>'
        };

        /// <summary>
        /// Dictionary of operator representations
        /// </summary>
        public static readonly Dictionary<Operators, string> Representations = new Dictionary<Operators, string>
        {
            { Operators.Equals, "==" },
            { Operators.NotEquals, "!=" },
            { Operators.GreaterThan, ">" },
            { Operators.LessThan, "<" },
            { Operators.Divide, "/" },
            { Operators.Multiply, "*" },
            { Operators.Add, "+" },
            { Operators.Subtract, "-" },
            { Operators.And, "&" },
            { Operators.Or, "|" }
        };

        /// <summary>
        /// Creates a new instance of the <see cref="Operator"/> class.
        /// </summary>
        /// <param name="operatorType"></param>
        public Operator(Operators operatorType)
        {
            m_operatorType = operatorType;
        }

        /// <summary>
        /// Calculate and return the result of the operation
        /// </summary>
        /// <returns></returns>
        public override object Result()
        {
            switch (m_operatorType)
            {
                case Operators.Divide:
                    return Divide(Left.Result(), Right.Result());
                case Operators.Multiply:
                    return Multiply(Left.Result(), Right.Result());
                case Operators.Add:
                    return Add(Left.Result(), Right.Result());
                case Operators.Subtract:
                    return Subtract(Left.Result(), Right.Result());
                case Operators.And:
                    return And(Left.Result(), Right.Result());
                case Operators.Or:
                    return Or(Left.Result(), Right.Result());
                case Operators.Equals:
                    return Equality(Left.Result(), Right.Result());
                case Operators.NotEquals:
                    return !Equality(Left.Result(), Right.Result());
                case Operators.LessThan:
                    return LessThan(Left.Result(), Right.Result());
                case Operators.GreaterThan:
                    return GreaterThan(Left.Result(), Right.Result());
                case Operators.None:
                    throw new ExpressionError("None operator is invalid");
                default:
                    throw new ExpressionError(string.Format("Unhandled operator '{0}'", m_operatorType));
            }
        }

        /// <summary>
        /// Operator to use
        /// </summary>
        public Operators OperatorType { get { return m_operatorType; } }

        /// <summary>
        /// Gets the text representation of the operation
        /// </summary>
        public override string Text
        {
            get { return string.Format("{0} {1} {2}", Left.Text, Representations[m_operatorType], Right.Text); }
        }

        private object Divide(object left, object right)
        {
            if (left.GetType().Name != right.GetType().Name)
            {
                throw new ExpressionError("Divide expects both types to be Int32");
            }

            if (left.GetType().Name != "Int32")
            {
                throw new ExpressionError("Divide expects both types to be Int32");
            }

            return (int)left / (int)right;
        }

        private object Multiply(object left, object right)
        {
            if (left.GetType().Name != right.GetType().Name)
            {
                throw new ExpressionError("Multiply expects both types to be Int32");
            }

            if (left.GetType().Name != "Int32")
            {
                throw new ExpressionError("Multiply expects both types to be Int32");
            }

            return (int)left * (int)right;
        }

        private object Add(object left, object right)
        {
            if (left.GetType().Name != right.GetType().Name)
            {
                throw new ExpressionError("Add expects both types to be Int32 or String");
            }

            switch (left.GetType().Name)
            {
                case "Int32":
                    return (int)left + (int)right;
                case "String":
                    return (string)left + (string)right;
                default:
                    throw new ExpressionError("Add expects both types to be Int32 or String");
            }
        }

        private object Subtract(object left, object right)
        {
            if (left.GetType().Name != right.GetType().Name)
            {
                throw new ExpressionError("Subtract expects both types to be Int32");
            }

            if (left.GetType().Name != "Int32")
            {
                throw new ExpressionError("Subtract expects both types to be Int32");
            }

            return (int)left - (int)right;
        }

        private object LessThan(object left, object right)
        {
            if (left.GetType().Name != right.GetType().Name)
            {
                throw new ExpressionError("LessThan expects both types to be Int32");
            }

            if (left.GetType().Name != "Int32")
            {
                throw new ExpressionError("LessThan expects both types to be Int32");
            }

            return (int)left < (int)right;
        }

        private object GreaterThan(object left, object right)
        {
            if (left.GetType().Name != right.GetType().Name)
            {
                throw new ExpressionError("GreaterThan expects both types to be Int32");
            }

            if (left.GetType().Name != "Int32")
            {
                throw new ExpressionError("GreaterThan expects both types to be Int32");
            }

            return (int)left > (int)right;
        }

        private bool Equality(object left, object right)
        {
            if (left.GetType().Name != right.GetType().Name)
            {
                throw new ExpressionError("Add expects both types to be Int32 or String");
            }

            switch (left.GetType().Name)
            {
                case "Int32":
                    return (int)left == (int)right;
                case "String":
                    return (string)left == (string)right;
                default:
                    throw new ExpressionError("Add expects both types to be Int32 or String");
            }
        }

        private bool And(object left, object right)
        {
            if (left.GetType().Name != right.GetType().Name)
            {
                throw new ExpressionError("And expects both types to be Boolean");
            }

            switch (left.GetType().Name)
            {
                case "Boolean":
                    return (bool)left && (bool)right;
                default:
                    throw new ExpressionError("And expects both types to be Boolean");
            }
        }

        private bool Or(object left, object right)
        {
            if (left.GetType().Name != right.GetType().Name)
            {
                throw new ExpressionError("Or expects both types to be Boolean");
            }

            switch (left.GetType().Name)
            {
                case "Boolean":
                    return (bool)left || (bool)right;
                default:
                    throw new ExpressionError("Or expects both types to be Boolean");
            }
        }
    }
}
