using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmicAlley
{
    public abstract class SubsetSumStrategy
    {
        public abstract List<int> Find(List<int> list, int sum);
    }

    public class EnumerateAllSubsets : SubsetSumStrategy
    {
        public override List<int> Find(List<int> list, int sum)
        {
            // stolen from http://igoro.com/archive/7-tricks-to-simplify-your-programs-with-linq/
            // only works on lists with length of 30 or less
            var subsets = from m in Enumerable.Range(0, 1 << list.Count)
                          select
                              from i in Enumerable.Range(0, list.Count)
                              where (m & (1 << i)) != 0
                              select list[i];

            var result = subsets.FirstOrDefault(set => set.Sum() == sum);
            if (result == null)
                return null;

            return result.ToList();
        } 
    }

    public class Greedy : SubsetSumStrategy
    {
        public override List<int> Find(List<int> list, int sum)
        {
            var result = new List<int>();
            foreach (int i in list.OrderByDescending(k => k).SkipWhile(k => k > sum))
            {
                if (result.Sum() + i <= sum)
                    result.Add(i);
            }

            return result;
        }
    }

    public class ExpandSubsets : SubsetSumStrategy
    {
        public override List<int> Find(List<int> list, int sum)
        {
            var subsets = new List<IEnumerable<int>> { new List<int>() };
            for (int i = 0; i < list.Count; i++)
            {
                var T = subsets.Select(y => y.Concat(new[] { list[i] }));

                subsets.AddRange(T.ToList());
                
                var result = subsets.Find(s => s.Sum() == sum);

                if (result != null)
                    return result.ToList();

            }

            return null;
        }
    }

    public class ApproximatingSubsets : SubsetSumStrategy
    {
        float _delta;

        public ApproximatingSubsets( float delta )
        {
            _delta = delta;
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

        public override List<int> Find(List<int> list, int sum)
        {
            var subsets = new List<IEnumerable<int>> { new List<int>() };
            for (int i = 0; i < list.Count; i++)
            {
                var T = subsets.Select(y => y.Concat(new[] { list[i] }));
                var U = subsets.Union(T).OrderBy(k => k.Sum()).ToList();

                subsets = RemoveCloseSums(U, _delta).Where( s => s.Sum() <= sum).ToList();
                var result = subsets.Find(s => s.Sum() <= sum && s.Sum() > (1 - _delta) * sum);

                if (result != null)
                    return result.ToList();

                var candidates = subsets.Select(s => s.Sum()).ToList();
            }

            return null;
        }
    }

    public class Scheduler
    {
        SubsetSumStrategy _strategy;

        public Scheduler(SubsetSumStrategy strategy)
        {
            _strategy = strategy;
        }

        public List<int> FindJobsWithTotalLength( List<int> allJobs, int length )
        {
            return _strategy.Find(allJobs, length);
        }

    }
}
