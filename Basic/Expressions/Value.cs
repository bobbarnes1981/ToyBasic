﻿using Basic.Types;

namespace Basic.Expressions
{
    /// <summary>
    /// Represents a value expression node
    /// </summary>
    public class Value : Node
    {
        private Type m_type;

        public Value(Type type)
        {
            m_type = type;
        }

        public override object Result()
        {
            return m_type.Value();
        }

        public override string Text
        {
            get { return m_type.Text; }
        }
    }
}
