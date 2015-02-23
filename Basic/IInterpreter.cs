namespace Basic
{
    public interface IInterpreter
    {
        IBuffer Buffer { get; }
        IConsole Console { get; }
        IHeap Heap { get; }
        IStack Stack { get; }
        void Execute();
        void Exit();
    }
}
