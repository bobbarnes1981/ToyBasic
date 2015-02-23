using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic
{
    public static class Extensions
    {
        private static Dictionary<string, OperatorType> m_operators = new Dictionary<string, OperatorType>
        {
            { "+", OperatorType.Add },
            { "-", OperatorType.Subtract },
            { "*", OperatorType.Multiply },
            { "/", OperatorType.Divide },
            { "==", OperatorType.Equals }
        };
        public static bool TryParseOperator(this string str, out OperatorType op)
        {
            if (m_operators.ContainsKey(str))
            {
                op = m_operators[str];
                return true;
            }
            op = OperatorType.None;
            return false;
        }
    }
}
