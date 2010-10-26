using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace AlgorithmicAlley
{
    public class AggregatingSplitter : Splitter
    {
        public override IEnumerable<string> Split(string ss)
        {
            string word = "";
            var words = ss.AsEnumerable().Aggregate(new List<string>(), (List<string> result, char c) =>
            {
                if (c == ' ')
                {
                    if (!string.IsNullOrEmpty(word))
                    {
                        result.Add(word);
                        word = "";
                    }
                }
                else
                    word += c;

                return result;
            }).ToList();

            if (!string.IsNullOrEmpty(word))
                words.Add(word);

            return words;
        }

    }
}
