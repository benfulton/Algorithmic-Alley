using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmicAlley
{
    public class Subset : IEquatable<Subset>
    {
        readonly List<int> list;
        readonly int sum;

        public Subset(List<int> aList)
        {
            list = aList;
            sum = list.Sum();
        }

        public Subset Union(int n)
        {
            return new Subset(list.Union(new[] { n }).ToList());
        }

        public int Sum()
        {
            return sum;
        }

        public List<int> ToList()
        {
            return list;
        }

        public override string ToString()
        {
            return String.Format("({0})", string.Join(",", list.Select(i => i.ToString()).ToArray()));
        }


        #region IEquatable<Subset> Members

        public override int GetHashCode()
        {
            return list.Aggregate(0, (i, j) => i ^ j);
        }

        public bool Equals(Subset other)
        {
            return list.Count() == other.list.Count() && !list.Except(other.list).Any();
        }

        #endregion

    }

}
