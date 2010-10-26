using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace AlgorithmicAlley
{
    public class NormalSplitter : Splitter
    {
        public override IEnumerable<string> Split(string ss)
        {
            var result = new List<string>();
            string word = "";
            for (int i = 0; i < ss.Length; ++i)
            {
                if (ss[i] == ' ')
                {
                    if (!string.IsNullOrEmpty(word))
                    {
                        result.Add(word);
                        word = "";
                    }
                }
                else
                    word += ss[i];
            }
            if (!string.IsNullOrEmpty(word))
                result.Add(word);

            return result;
        }
    }
}
