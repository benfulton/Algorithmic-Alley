using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Drawing;
using AlgorithmicAlley;

namespace AlgorithmicAlleyTests
{
    public class ClosestPointTests
    {
        IEnumerable<PointF> CreatePoints(int count)
        {
            var randomizer = new Random(10);
            var points = Enumerable.Range(0, count)
                .Select( i => new PointF( (float)randomizer.NextDouble(), (float)randomizer.NextDouble())).ToList();

            return points;
        }

        [Fact]
        public void A_list_with_two_points_returns_them_as_closest()
        {
            var points = CreatePoints(2).ToList();
            Segment closest = new ClosestPointSolver().Closest_BruteForce(points);
            Assert.Equal(closest.P1, points[0]);
            Assert.Equal(closest.P2, points[1]);
        }

        [Fact]
        public void a_list_with_1000_points_finds_the_closest()
        {
            var points = CreatePoints(1000).ToList();
            var expected = new Segment(new PointF(0.7874735f, 0.9796776f), new PointF(0.786889f, 0.980248451f));
            Segment closest = new ClosestPointSolver().Closest_BruteForce(points);
            Assert.Equal(expected, closest);
        }
    }
}
