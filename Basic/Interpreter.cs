using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic
{
    public class Interpreter : IInterpreter
    {
        private string m_prompt = "> ";

        private bool m_running = true;

        private bool m_executing = false;

        private string m_version = "0.1";

        private IParser m_parser;

        private IBuffer m_buffer;

        private IDisplay m_display;

        private IHeap m_heap;

        public Interpreter(IBuffer buffer, IParser parser, IDisplay display, IHeap heap)
        {
            m_buffer = buffer;
            m_parser = parser;
            m_display = display;
            m_heap = heap;
        }

        public IBuffer Buffer
        {
            get { return m_buffer; }
        }

        public IDisplay Display
        {
            get { return m_display; }
        }

        public IHeap Heap
        {
            get { return m_heap; }
        }

        public void Run()
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            Console.WriteLine("Basic Interpreter\r\nVersion {0}\r\n(c) Bob Barnes 2015", m_version);

            Line line;
            do
            {
                Console.Write(m_prompt);
                try
                {
                    line = m_parser.Parse(Console.ReadLine());
                }
                catch(Error error)
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
            } while (m_running);
        }

        private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Display.Output(string.Format("Interrupt {0}", e.SpecialKey));
            m_executing = false;
            e.Cancel = true;
        }

        private void displayError(Error error)
        {
            Console.WriteLine("{0}: {1}", error.GetType(), error.Message);
        }

        public void Execute()
        {
            m_buffer.Reset();
            m_executing = true;
            while (!m_buffer.End && m_executing)
            {
                m_buffer.Fetch.Command.Execute(this);
            }
            Display.Output("Done");
        }

        public void Exit()
        {
            m_running = false;
        }
    }
}
