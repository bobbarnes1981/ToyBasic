using Basic.Errors;
using Basic.Expressions;
using Basic.Types;

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
                throw new CommandError("Invalid gosub_return in current stack frame");
            }

            IFrame frame = interpreter.Stack.Peek();
            interpreter.Buffer.Jump(frame.Get<int>("gosub_return"));
        }

        public override string Text
        {
            get { return Keywords.Return.ToString(); }
        }
    }
}
