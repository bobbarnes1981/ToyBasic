﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic
{
    public class Error : Exception
    {
        public Error(string message)
            : base(message)
        {
        }
    }
}
