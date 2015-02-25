using System.Collections.Generic;

namespace Basic.Expressions
{
    public class Operator : Expression
    {
        private readonly Operators m_operatorType;

        /// <summary>
        /// Valid operator characters
        /// </summary>
        public static readonly List<char> VALID_CHARACTERS = new List<char>
        {
            '+', '-', '=', '*', '/', '!'
        };

        public static readonly Dictionary<Operators, string> Representations = new Dictionary<Operators, string>
        {
            { Operators.Equals, "==" },
            { Operators.Divide, "/" },
            { Operators.Multiply, "*" },
            { Operators.Add, "+" },
            { Operators.Subtract, "-" },
        };

        public Operator(Operators operatorType)
        {
            m_operatorType = operatorType;
        }

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
                case Operators.Equals:
                    return Equals(Left.Result(), Right.Result());
                case Operators.None:
                    throw new Errors.Expression("None operator is invalid");
                default:
                    throw new Errors.Expression(string.Format("Unhandled operator '{0}'", m_operatorType));
            }
        }

        public Operators OperatorType { get { return m_operatorType; }}

        public override string Text
        {
            get { return string.Format("{0} {1} {2}", Left.Text, Representations[m_operatorType], Right.Text ); }
        }

        private object Divide(object left, object right)
        {
            if (left.GetType().Name != right.GetType().Name)
            {
                throw new Errors.Expression("Divide expects both types to be Int32");
            }
            if (left.GetType().Name != "Int32")
            {
                throw new Errors.Expression("Divide expects both types to be Int32");
            }
            return (int)left / (int)right;
        }

        private object Multiply(object left, object right)
        {
            if (left.GetType().Name != right.GetType().Name)
            {
                throw new Errors.Expression("Multiply expects both types to be Int32");
            }
            if (left.GetType().Name != "Int32")
            {
                throw new Errors.Expression("Multiply expects both types to be Int32");
            }
            return (int)left * (int)right;
        }

        private object Add(object left, object right)
        {
            if (left.GetType().Name != right.GetType().Name)
            {
                throw new Errors.Expression("Add expects both types to be Int32 or String");
            }
            switch (left.GetType().Name)
            {
                case "Int32":
                    return (int) left + (int) right;
                case "String":
                    return (string) left + (string) right;
                default:
                    throw new Errors.Expression("Add expects both types to be Int32 or String");
            }
        }

        private object Subtract(object left, object right)
        {
            if (left.GetType().Name != right.GetType().Name)
            {
                throw new Errors.Expression("Subtract expects both types to be Int32");
            }
            if (left.GetType().Name != "Int32")
            {
                throw new Errors.Expression("Subtract expects both types to be Int32");
            }
            return (int)left - (int)right;
        }

        private object Equals(object left, object right)
        {
            if (left.GetType().Name != right.GetType().Name)
            {
                throw new Errors.Expression("Add expects both types to be Int32 or String");
            }
            switch (left.GetType().Name)
            {
                case "Int32":
                    return (int)left == (int)right;
                case "String":
                    return (string)left == (string)right;
                default:
                    throw new Errors.Expression("Add expects both types to be Int32 or String");
            }
        }
    }
}
