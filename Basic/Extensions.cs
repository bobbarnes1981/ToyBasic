using System.Linq;
using Basic.Expressions;

namespace Basic
{
    public static class Extensions
    {
        public static bool TryParseOperator(this string str, out Operators op)
        {
            if (Operator.Representations.ContainsValue(str))
            {
                op = Operator.Representations.First(x => x.Value == str).Key;
                return true;
            }
            op = Operators.None;
            return false;
        }
    }
}
