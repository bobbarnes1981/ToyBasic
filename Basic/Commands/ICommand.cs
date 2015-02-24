namespace Basic.Commands
{
    public interface ICommand
    {
        Keywords Keyword { get; }
        bool IsSystem { get; }
        void Execute(IInterpreter interpreter);
        string Text { get; }
    }
}
