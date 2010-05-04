using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmicAlley
{
    public class Scheduler
    {
        public List<int> SubsetFor(List<int> list, int sum)
        {
            // stolen from http://igoro.com/archive/7-tricks-to-simplify-your-programs-with-linq/
            // only works on lists with length of 30 or less
             var subsets = from m in Enumerable.Range(0, 1 << list.Count)
              select
                  from i in Enumerable.Range(0, list.Count)
                  where (m & (1 << i)) != 0
                  select list[i];

             return subsets.First(set => set.Sum() == sum).ToList();
        }

        public List<int> GreedySubsetFor(List<int> list, int sum)
        {
            var result = new List<int>();
            foreach (int i in list.OrderByDescending(k => k).SkipWhile( k => k > sum))
            {
                if (result.Sum() + i > sum)
                    return result;
                else
                    result.Add(i);
            }

            return result;
            
        }
        IEnumerable<IEnumerable<int>> RemoveCloseSums(List<IEnumerable<int>> list, float c)
        {
            var y = list.First();
            yield return y;

            foreach (var i in list)
            {
                if (y.Sum() < (1 - c / list.Count) * i.Sum())
                {
                    y = i;
                    yield return i;
                }
            }
        }

        public List<int> ApproximateSubsetFor( List<int> list, int sum, float c)
        {
            var subsets = new List<IEnumerable<int>> { new List<int>() };
            for (int i = 0; i < list.Count; i++)
            {
            	var T = list.SelectMany( j => subsets.Select( k => k.Union(new[]{j})));
                var U = subsets.Union(T).OrderBy( k => k.Sum() ).ToList();

                subsets = RemoveCloseSums(U, c).ToList();
                var result = subsets.Find(s => s.Sum() <= sum && s.Sum() > (1 - c) * sum);
                if (result != null)
                    return result.ToList();
            }

            return null;
        }
    }
}
