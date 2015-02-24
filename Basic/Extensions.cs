using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic
{
    public static class Extensions
    {
        private static Dictionary<string, Operators> m_operators = new Dictionary<string, Operators>
        {
            { "+", Operators.Add },
            { "-", Operators.Subtract },
            { "*", Operators.Multiply },
            { "/", Operators.Divide },
            { "==", Operators.Equals }
        };
        public static bool TryParseOperator(this string str, out Operators op)
        {
            if (m_operators.ContainsKey(str))
            {
                op = m_operators[str];
                return true;
            }
            op = Operators.None;
            return false;
        }
    }
}
