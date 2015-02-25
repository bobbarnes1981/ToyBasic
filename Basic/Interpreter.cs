﻿using System;
using System.Reflection;
using Basic.Parsers;

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

        private IParser<ILine> m_parser;

        private IBuffer m_buffer;

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
        /// <param name="parser"></param>
        public Interpreter(IBuffer buffer, IConsole console, IHeap heap, IStack stack, IStorage storage, IParser<ILine> parser)
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

            if (parser == null)
                throw new ArgumentNullException("parser");
            m_parser = parser;
        }

        public IBuffer Buffer
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
                line = m_parser.Parse(this, new TextStream(input));
            }
            catch(Errors.Error error)
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
                    catch (Errors.Error error)
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
        private void displayError(Errors.Error error)
        {
            // TODO: colours
            m_console.Output(string.Format("{0}: {1}\r\n", error.GetType(), error.Message));
        }

        /// <summary>
        /// Execute the program in the interpreter buffer
        /// </summary>
        public void Execute()
        {
            m_buffer.Reset();
            m_executing = true;
            while (!m_buffer.End && m_executing)
            {
                m_buffer.Fetch.Command.Execute(this);
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
