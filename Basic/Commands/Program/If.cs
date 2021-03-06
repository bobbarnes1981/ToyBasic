﻿using Basic.Errors;
using Basic.Expressions;

namespace Basic.Commands.Program
{
    /// <summary>
    /// Represents the program command 'If'
    /// </summary>
    public class If : Command
    {
        /// <summary>
        /// The expression to evaluate
        /// </summary>
        private readonly INode m_expression;

        /// <summary>
        /// The command to execute if the expression is true
        /// </summary>
        private readonly ICommand m_command;

        /// <summary>
        /// Creates a new instance of the <see cref="If"/> class.
        /// </summary>
        /// <param name="expression">The expression to evaluate</param>
        /// <param name="command">The command to execute if the expression is true</param>
        public If(INode expression, ICommand command)
            : base(Keywords.If, false)
        {
            m_expression = expression;
            m_command = command;
        }

        public ICommand Command
        {
            get { return m_command; }
        }

        /// <summary>
        /// Executes the 'If' command by evaluating the expression and executing the command if the expression is true
        /// </summary>
        /// <param name="interpreter">interpreter to execute the command on</param>
        public override void execute(IInterpreter interpreter)
        {
            object result = m_expression.Result(interpreter);
            bool output;
            switch (result.GetType().Name)
            {
                case "Int32":
                    output = (int)result != 0;
                    break;
                case "Boolean":
                    output = (bool)result;
                    break;
                case "String":
                    output = !string.IsNullOrEmpty((string)result);
                    break;
                default:
                    throw new CommandError(string.Format("Unhandled type '{0}'", result.GetType().Name));
            }

            if (output)
            {
                m_command.Execute(interpreter);
            }
        }

        /// <summary>
        /// Gets the text representation of the command
        /// </summary>
        public override string Text
        {
            get { return string.Format("{0} {1} {2} {3}", Keywords.If, m_expression.Text, Keywords.Then, m_command.Text); }
        }
    }
}
