namespace Basic
{
    /// <summary>
    /// Interpreter interface
    /// </summary>
    public interface IInterpreter
    {
        /// <summary>
        /// Line buffer
        /// </summary>
        IBuffer Buffer { get; }

        /// <summary>
        /// Console input/output
        /// </summary>
        IConsole Console { get; }

        /// <summary>
        /// Heap storage
        /// </summary>
        IHeap Heap { get; }

        /// <summary>
        /// Stack storage
        /// </summary>
        IStack Stack { get; }

        /// <summary>
        /// Program storage
        /// </summary>
        IStorage Storage { get; }

        /// <summary>
        /// Execute the program in the buffer
        /// </summary>
        void Execute();

        /// <summary>
        /// Run interpreter
        /// </summary>
        void Run();

        /// <summary>
        /// Parse and store/execute a line of input
        /// </summary>
        /// <param name="input"></param>
        void ProcessInput(string input);

        /// <summary>
        /// Exit the interpreter
        /// </summary>
        void Exit();
    }
}
