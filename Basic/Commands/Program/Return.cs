using Basic.Errors;

namespace Basic.Commands.Program
{
    /// <summary>
    /// Represents the program command 'Return'
    /// </summary>
    public class Return : Command
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Return"/> class.
        /// </summary>
        public Return()
            : base(Keywords.Return, false)
        {
        }

        public override void execute(IInterpreter interpreter)
        {
            if (interpreter.Stack.Count == 0
                || !interpreter.Stack.Peek().Exists("gosub_return"))
            {
                throw new CommandError("Missing gosub_return in current stack frame");
            }

            IFrame frame = interpreter.Stack.Pop();
            interpreter.Buffer.Jump(frame.Get<int>("gosub_return"));
            interpreter.Buffer.Next();
        }

        public override string Text
        {
            get { return Keywords.Return.ToString(); }
        }
    }
}
