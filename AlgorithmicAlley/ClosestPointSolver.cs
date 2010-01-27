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
    }
}
