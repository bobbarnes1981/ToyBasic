namespace Basic.Commands
{
    public interface ICommand
    {
        Keyword Keyword { get; }
        void Execute(IInterpreter interpreter);
        string Text { get; }
    }
}
