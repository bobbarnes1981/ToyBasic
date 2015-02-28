using Basic.Errors;
using Basic.Expressions;
using Basic.Types;

namespace Basic.Commands.Program
{
    public class Dim : Command
    {
        private readonly Variable m_variable;

        public Dim(Variable variable)
            : base(Keywords.Dim, false)
        {
            m_variable = variable;
        }

        public override void execute(IInterpreter interpreter)
        {
            object value = m_variable.Index.Result(interpreter);
            if (value.GetType() != typeof (int))
            {
                throw new CommandError("Dim expects type to be Number");
            }

            interpreter.Heap.Set(m_variable.Name, new object[(int)value]);
        }

        public override string Text
        {
            get { return string.Format("{0} {1}[{2}]", Keywords.Dim, m_variable.Text, m_variable.Index.Text); }
        }
    }
}
