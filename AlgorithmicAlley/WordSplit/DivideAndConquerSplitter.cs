using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmicAlley
{
    public class DivideAndConquerSplitter : ParallelizingSplitter
    {
        public override IEnumerable<string> Split(string s)
        {
            return Reduce(Map(s), s.Length).Finalize();
        }

        public IWordState Reduce(IEnumerable<IWordState> set, int len)
        {
            if (len == 0)
                return new Chunk("");

            if (len == 1)
                return set.First();
            else
            {
                int pivot = len / 2;

                //var i1 = set.Take(pivot).ToList();
                //var i2 = set.Skip(pivot).ToList();

                //var t1 = new Task<IWordState>(() => Reduce(i1, pivot));
                //var t2 = new Task<IWordState>(() => Reduce(i2, pivot + len % 2));

                var firstHalf = Reduce(set.Take(pivot), pivot);
                var secondHalf = Reduce(set.Skip(pivot), pivot + len % 2);

                IWordState result = firstHalf.Combine(secondHalf);

                return result;
            }
        }
    }
}
