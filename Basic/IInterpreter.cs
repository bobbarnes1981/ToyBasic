namespace Basic
{
    public interface IInterpreter
    {
        IBuffer Buffer { get; }
        IConsole Console { get; }
        IHeap Heap { get; }
        void Execute();
        void Exit();
    }
}
