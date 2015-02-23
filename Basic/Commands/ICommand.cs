namespace Basic.Commands
{
    public interface ICommand
    {
        Keyword Keyword { get; }
        bool IsSystem { get; }
        void Execute(IInterpreter interpreter);
        string Text { get; }
    }
}
