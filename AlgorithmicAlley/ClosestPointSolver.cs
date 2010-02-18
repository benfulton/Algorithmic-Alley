using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace AlgorithmicAlley
{
    public class Segment
    {
        public Segment(PointF p1, PointF p2)
        {
            P1 = p1;
            P2 = p2;
        }

        public readonly PointF P1;
        public readonly PointF P2;

        public float Length()
        {
            return (float)Math.Sqrt(LengthSquared());
        }

        public float LengthSquared()
        {
            return (P1.X - P2.X) * (P1.X - P2.X)
                + (P1.Y - P2.Y) * (P1.Y - P2.Y);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Segment;
            if (other == null) return false;

            return P1 == other.P1 && P2 == other.P2;
        }

        public override string ToString()
        {
            return String.Format("{0} - {1}", P1, P2);
        } 
    }
    public class Buckets
    {
        private readonly float _IntervalSize;
        Dictionary<int, List<float>> buckets = new Dictionary<int, List<float>>();

        public Buckets(float intervalSize)
        {
            _IntervalSize = intervalSize;
        }

        public int Bucket(float r)
        {
            return (int)(r / _IntervalSize);
        }
        public void Add(int bucket, float r)
        {
            if (!buckets.ContainsKey(bucket))
            {
                buckets[bucket] = new List<float>();
            }
            buckets[bucket].Add(r);
        }

        IEnumerable<float> Get(int id)
        {
            if (buckets.ContainsKey(id))
                return buckets[id];
            else
                return new List<float>();
        }

        public bool HasBucketLargerThan(double sz)
        {
            return buckets.Values.FirstOrDefault(items => items.Count > sz) != null;
        }
        public IEnumerable<List<float>> BucketsWithMoreThanOneItem()
        {
            return buckets.Values.Where(items => items.Count > 1);
        }

        public List<int> GetBuckets()
        {
            return buckets.Keys.OrderBy(i => i).ToList();
        }

        public IEnumerable<float> PointsNearBucket(int id)
        {
            return Enumerable.Range(id - 1, 3).SelectMany(i => Get(i));
        }

        public struct Pair
        {
            public Pair(float t1, float t2)
            {
                first = t1;
                second = t2;
            }
            public float first;
            public float second;
        }

        public IEnumerable<Pair> CandidatesInBucket(int id)
        {
            var list = PointsNearBucket(id).ToList();
            return Enumerable.Range(0, list.Count())
                .SelectMany(i => Enumerable.Range(i + 1, list.Count() - (i + 1))
                    .Select(j => new Pair(list[i], list[j])));
        }


    }

    public class ClosestPointSolver
    {
        public Segment Closest_BruteForce(List<PointF> points)
        {
            int n = points.Count;
            var allPairs = Enumerable.Range(0, n - 1)
                .SelectMany(i => Enumerable.Range(i + 1, n - (i + 1))
                    .Select(j => new Segment(points[i], points[j])));

            return allPairs.OrderBy(seg => seg.LengthSquared())
                    .First();
        }

        public Segment Closest_Recursive(List<PointF> points)
        {
            if (points.Count() < 4) return Closest_BruteForce(points);

            int split = points.Count() / 2;
            var ordered = points.OrderBy(point => point.X);     // sort the points by their x value
            var pointsOnLeft = ordered.Take(split).ToList();    // split into two groups, those to the
            var pointsOnRight = ordered.Skip(split).ToList();   // left and those to the right

            var leftMin = Closest_Recursive(pointsOnLeft);
            var rightMin = Closest_Recursive(pointsOnRight);

            float minDist = Math.Min(leftMin.Length(), rightMin.Length());

            var xDivider = pointsOnLeft.Last().X;
            var closeY = pointsOnRight.TakeWhile(point => point.X - xDivider < minDist).OrderBy(point => point.Y);

            var crossingPairs = pointsOnLeft.SkipWhile(point => xDivider - point.X > minDist)
                .SelectMany(p1 => closeY.SkipWhile(i => i.Y < p1.Y - minDist)
                    .TakeWhile(i => i.Y < p1.Y + minDist)
                .Select(p2 => new Segment(p1, p2)));

            return crossingPairs.Union(new[] { leftMin, rightMin })
                .OrderBy(segment => segment.Length()).First();
        }

        float FindIntervalSize(List<float> S)
        {
            float intervalsize = (S.Max() - S.Min()) / S.Count;
            var T = new List<float>(S);
            while (T.Count > 0)
            {
                Buckets buckets = new Buckets(intervalsize);
                while (T.Count > 0 && !buckets.HasBucketLargerThan(Math.Sqrt(S.Count)))
                {
                    float r = T[T.Count - 1];
                    T.RemoveAt(T.Count - 1);
                    int bucket = buckets.Bucket(r);
                    buckets.Add(bucket, r);
                }

                intervalsize = buckets.BucketsWithMoreThanOneItem()
                    .Select(items => FindIntervalSize(items))
                    .Union(new[] { intervalsize })
                    .Min();
            }

            Buckets morebuckets = new Buckets(intervalsize);
            S.ForEach(r => morebuckets.Add(morebuckets.Bucket(r), r));
            intervalsize = morebuckets.BucketsWithMoreThanOneItem()
                .Select(items => FindIntervalSize(items))
                .Union(new[] { intervalsize })
                .Min();

            return intervalsize;
        }

        public PointF ClosestFloats(List<float> floats)
        {
            float intervalsize = FindIntervalSize(floats);
            var buckets = new Buckets(2.0f * intervalsize);
            floats.ForEach(r => buckets.Add(buckets.Bucket(r), r));
            var ordered = buckets.GetBuckets();
            var pair = ordered.SelectMany(i => buckets.CandidatesInBucket(i))
                .OrderBy(p => Math.Abs(p.first - p.second))
                .First();

            return new PointF(pair.first, pair.second);
        }

        // Here's a simple implementation of finding closest floats 
        // by comparing all possible pairs
        public PointF ClosestFloats_BruteForce(List<float> floats)
        {
            IEnumerable<PointF> list = Enumerable.Range(0, floats.Count)
                            .SelectMany(i => Enumerable.Range(i + 1, floats.Count - (i + 1))
                            .Select(j => new PointF(floats[i], floats[j])));
            return list
                .OrderBy(p => Math.Abs(p.X - p.Y))
                .First();
        }

    }
}
