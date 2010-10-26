using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace AlgorithmicAlley
{
    public class GroupingSplitter : ParallelizingSplitter
    {
        private const int INT_PARALLEL_CHUNKS = 100;

        public override IEnumerable<string> Split(string s)
        {
            var subsets = Reduce(Map(s)).ToList();
            while (subsets.Count > INT_PARALLEL_CHUNKS)
            {
                subsets = Reduce(subsets).ToList();
            }

            if (subsets.Any())
            {
                var final = subsets.Aggregate((runningProduct, nextFactor) => runningProduct.Combine(nextFactor));

                return final.Finalize();
            }

            return new List<string>();
        }

        public IEnumerable<IWordState> Reduce(IEnumerable<IWordState> set)
        {
            var groups = set.Select((ws, index) => new { ws, index })
                .GroupBy(g => g.index / INT_PARALLEL_CHUNKS, i => i.ws);

            return groups.AsParallel().AsOrdered()
                .Select(batch => batch.Aggregate((runningProduct, nextFactor) => runningProduct.Combine(nextFactor)));

        }
    }
}
