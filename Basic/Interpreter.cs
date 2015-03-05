using System;
using System.Reflection;
using Basic.Errors;
using Basic.Parser;
using Basic.Tokenizer;

namespace Basic
{
    /// <summary>
    /// Represents the basic interpreter
    /// </summary>
    public class Interpreter : IInterpreter
    {
        private string m_prompt = "> ";

        private bool m_running = true;

        private bool m_executing = false;

        private ITokenizer m_tokenizer;

        private IParser m_parser;

        private ILineBuffer m_buffer;

        private IConsole m_console;

        private IHeap m_heap;

        private IStack m_stack;

        private IStorage m_storage;

        /// <summary>
        /// Creates a new instance of the <see cref="Interpreter"/> class.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="console"></param>
        /// <param name="heap"></param>
        /// <param name="stack"></param>
        /// <param name="storage"></param>
        /// <param name="tokenizer"></param>
        /// <param name="parser"></param>
        public Interpreter(ILineBuffer buffer, IConsole console, IHeap heap, IStack stack, IStorage storage, ITokenizer tokenizer, IParser parser)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            m_buffer = buffer;

            if (console == null)
                throw new ArgumentNullException("console");
            m_console = console;

            if (heap == null)
                throw new ArgumentNullException("heap");
            m_heap = heap;

            if (stack == null)
                throw new ArgumentNullException("stack");
            m_stack = stack;

            if (storage == null)
                throw new ArgumentNullException("storage");
            m_storage = storage;

            if (tokenizer == null)
                throw new ArgumentNullException("tokenizer");
            m_tokenizer = tokenizer;

            if (parser == null)
                throw new ArgumentNullException("parser");
            m_parser = parser;
        }

        public ILineBuffer Buffer
        {
            get { return m_buffer; }
        }

        public IConsole Console
        {
            get { return m_console; }
        }

        public IHeap Heap
        {
            get { return m_heap; }
        }

        public IStack Stack
        {
            get { return m_stack; }
        }

        public IStorage Storage
        {
            get { return m_storage; }
        }

        /// <summary>
        /// Run the interpreter
        /// </summary>
        public void Run()
        {
            System.Console.CancelKeyPress += Console_CancelKeyPress; // TODO: make this part of IConsole
            m_console.Output(string.Format("Basic Interpreter\r\nVersion {0}\r\nBob Barnes 2015\r\n", Assembly.GetEntryAssembly().GetName().Version));

            do
            {
                m_console.Output(m_prompt);
                ProcessInput(m_console.Input());
            } while (m_running);
        }

        /// <summary>
        /// Parse and store/execute a line of input
        /// </summary>
        /// <param name="input"></param>
        public void ProcessInput(string input)
        {
            ILine line;
            try
            {
                ITokenCollection tokens = m_tokenizer.Tokenize(new TextStream(input));
                line = m_parser.Parse(tokens);
            }
            catch (Error error)
            {
                line = null;
                displayError(error);
            }
            if (line != null)
            {
                if (line.Number == 0)
                {
                    try
                    {
                        line.Command.Execute(this);
                    }
                    catch (Error error)
                    {
                        displayError(error);
                    }
                }
                else
                {
                    m_buffer.Add(line);
                }
            }
        }

        /// <summary>
        /// Handle the control-c and control-break keypresses
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.Output(string.Format("Interrupt {0}\r\n", e.SpecialKey));
            m_executing = false;
            e.Cancel = true;
        }

        /// <summary>
        /// Display an error message
        /// </summary>
        /// <param name="error"></param>
        private void displayError(Error error)
        {
            // TODO: colours
            m_console.Output(string.Format("{0}: {1}\r\n", error.GetType(), error.Message));
            if (error.InnerError != null)
            {
                displayError(error.InnerError);
            }
        }

        /// <summary>
        /// Execute the program in the interpreter buffer
        /// </summary>
        public void Execute(bool debug)
        {
            m_buffer.Reset();
            m_executing = true;
            while (!m_buffer.End && m_executing)
            {
                m_buffer.Next();
                try
                {
                    if (debug)
                    {
                        m_console.Output(string.Format("DEBUG: {0}\t{1}\r\n", m_buffer.Current.Number, m_buffer.Current.Command.Text));
                        if (m_buffer.Current.Command.Keyword == Keywords.Break)
                        {
                            m_console.Output("Enter to continue...");
                            m_console.Input();
                        }
                    }
                    m_buffer.Current.Command.Execute(this);
                }
                catch (Error error)
                {
                    throw new InterpreterError(string.Format("Error at line '{0}'", m_buffer.Current.Number), error);
                }
            }

            m_console.Output("Done\r\n");
        }

        /// <summary>
        /// Exit the interpreter
        /// </summary>
        public void Exit()
        {
            m_running = false;
        }
    }
}
