﻿using System;
using System.Reflection;

namespace Basic
{
    public class Interpreter : IInterpreter
    {
        private string m_prompt = "> ";

        private bool m_running = true;

        private bool m_executing = false;

        private IParser m_parser;

        private IBuffer m_buffer;

        private IConsole m_console;

        private IHeap m_heap;

        public Interpreter(IBuffer buffer, IParser parser, IConsole display, IHeap heap)
        {
            m_buffer = buffer;
            m_parser = parser;
            m_console = display;
            m_heap = heap;
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

        public void Run()
        {
            System.Console.CancelKeyPress += Console_CancelKeyPress; // TODO: make this part of IConsole
            m_console.Output(string.Format("Basic Interpreter\r\nVersion {0}\r\nBob Barnes 2015\r\n", Assembly.GetEntryAssembly().GetName().Version));

            Line line;
            do
            {
                m_console.Output(m_prompt);
                try
                {
                    line = m_parser.Parse(this, m_console.Input());
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
            } while (m_running);
        }

        private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.Output(string.Format("Interrupt {0}\r\n", e.SpecialKey));
            m_executing = false;
            e.Cancel = true;
        }

        private void displayError(Errors.Error error)
        {
            m_console.Output(string.Format("{0}: {1}\r\n", error.GetType(), error.Message));
        }

        public void Execute()
        {
            m_buffer.Reset();
            m_executing = true;
            while (!m_buffer.End && m_executing)
            {
                m_buffer.Fetch.Command.Execute(this);
            }
            Console.Output("Done\r\n");
        }

        public void Exit()
        {
            m_running = false;
        }
    }
}
