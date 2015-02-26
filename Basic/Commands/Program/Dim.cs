namespace Basic.Commands.Program
{
    public class Dim : Command
    {
        private readonly string m_variable;

        public Dim(string variable)
            : base(Keywords.Dim, false)
        {
            m_variable = variable;
        }

        public override void execute(IInterpreter interpreter)
        {
            throw new global::System.NotImplementedException();
        }

        public override string Text
        {
            get { throw new global::System.NotImplementedException(); }
        }
    }
}
