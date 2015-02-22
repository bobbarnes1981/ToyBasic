using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic
{
    public static class Extensions
    {
        private static Dictionary<string, Operator> m_operators = new Dictionary<string, Operator>
        {
            { "+", Operator.Add },
            { "-", Operator.Subtract },
            { "*", Operator.Multiply },
            { "/", Operator.Divide },
            { "==", Operator.Equals }
        };
        public static bool TryParseOperator(this string str, out Operator op)
        {
            if (m_operators.ContainsKey(str))
            {
                op = m_operators[str];
                return true;
            }
            op = Operator.None;
            return false;
        }
    }
}
