﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LASI.Algorithm;
using LASI.ContentSystem;
using LASI.Utilities;
using System.IO;
using LASI.Algorithm.DocumentConstructs;
using LASI.Algorithm.LexicalLookup;

namespace Dustin_Experimentation
{ //this is a comment 
    class Program
    {
        static void Main(string[] args) {
            foreach (var t in LexicalLookup.UnstartedLoadingTasks) {
                t.Wait();
            }

        }
    }
}
