﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Errors
{
    public class ExpressionError : Error
    {
        public ExpressionError(string message)
            : base(message)
        {
        }
    }
}
