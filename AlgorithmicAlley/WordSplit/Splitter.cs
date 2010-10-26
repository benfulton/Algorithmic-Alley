using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace AlgorithmicAlley
{
    public abstract class Splitter
    {
        public abstract IEnumerable<string> Split(string stringToSplit);
    }
}