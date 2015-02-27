using Basic.Errors;
using Basic.Expressions;
using Basic.Types;

namespace Basic.Commands.Program
{
    public class Dim : Command
    {
        private readonly Variable m_variable;
        private readonly INode m_size;

        public Dim(Variable variable, INode size)
            : base(Keywords.Dim, false)
        {
            m_variable = variable;
            m_size = size;
        }

        public override void execute(IInterpreter interpreter)
        {
            object value = m_size.Result();
            if (value.GetType() != typeof (int))
            {
                throw new CommandError("Dim expects type to be Number");
            }

            interpreter.Heap.Set(m_variable.Text, new object[(int)value]);
        }

        public override string Text
        {
            get { return string.Format("{0} {1}[{2}]", Keywords.Dim, m_variable.Text, m_size.Text); }
        }
    }
}
