﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LASI.Algorithm
{
    public interface IQuantifiable
    {
        IQuantifier Quantifier
        {
            get;
            set;
        }
    }
}
