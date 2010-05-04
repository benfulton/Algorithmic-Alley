using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using AlgorithmicAlley;

namespace AlgorithmicAlleyTests
{
    class SubsetSumTests
    {

        [Fact]
        public void A_subset_of_one_and_two_sums_to_one()
        {
            var list = new List<int> { 1, 2 };
            Assert.Equal( 2, new Scheduler().SubsetFor(list, 2).Sum());
        }

        [Fact]
        public void Can_find_subset_of_14_from_5_elements()
        {
            var list = new List<int> { 1, 2,3,4,10 };
            Assert.Equal( 14, new Scheduler().SubsetFor(list, 14).Sum());
        }

        [Fact]
        public void Can_find_subset_of_668_from_10_elements()
        {
            var list = new List<int> { 1, 2, 92, 437, 16, 46, 991, 12, 92, 130 };
            Assert.Equal(668, new Scheduler().SubsetFor(list, 668).Sum());
        }

        [Fact]
        public void Greedy_can_find_approximate_subset_of_668_from_10_elements()
        {
            var list = new List<int> { 1, 2, 92, 437, 16, 46, 991, 12, 92, 130 };
            float delta = 0.02f;
            var result = new Scheduler().GreedySubsetFor(list, 668);
            Assert.InRange(result.Sum(), 668 * (1 - delta), 668 * (1 + delta));
        }

        [Fact]
        public void Can_find_approximate_subset_of_668_from_10_elements()
        {
            var list = new List<int> { 1, 2, 92, 437, 16, 46, 991, 12, 92, 130 };
            float delta = 0.01f;
            List<int> result = new Scheduler().ApproximateSubsetFor(list, 668, delta);
            Assert.InRange(result.Sum(), 668 * (1 - delta), 668 * (1 + delta));
        }

        [Fact]
        public void Greedy_can_find_subset_of_668_from_24_random_elements()
        {
            var randomizer = new Random(10);
            var list = Enumerable.Range(0, 24).Select(i => randomizer.Next(100)).ToList();
            float delta = 0.1f;
            var result = new Scheduler().GreedySubsetFor(list, 668);
            Assert.InRange(result.Sum(), 668 * (1 - delta), 668 * (1 + delta));
        }

        [Fact]
        public void Can_find_approximate_subset_of_668_from_24_random_elements()
        {
            var randomizer = new Random(10);
            var list = Enumerable.Range(0, 24).Select(i => randomizer.Next(100)).ToList();
            float delta = 0.01f;
            List<int> result = new Scheduler().ApproximateSubsetFor(list, 668, delta);
            Assert.InRange(result.Sum(), 668 * (1 - delta), 668 * (1 + delta));
        }

        [Fact]
        public void Can_find_exact_subset_of_668_from_24_random_elements()
        {
            var randomizer = new Random(10);
            var list = Enumerable.Range(0, 24).Select(i => randomizer.Next(100)).ToList();
            float delta = 0.01f;
            Assert.Throws<OutOfMemoryException>( () => { new Scheduler().SubsetFor(list, 668); });
        }

        [Fact]
        public void Greedy_can_find_approximate_subset_of_668_from_1000_random_elements()
        {
            var randomizer = new Random(10);
            var list = Enumerable.Range(0, 1000).Select(i => randomizer.Next(100)).ToList();
            float delta = 0.2f;
            var result = new Scheduler().GreedySubsetFor(list, 668);
            Assert.InRange(result.Sum(), 668 * (1 - delta), 668 * (1 + delta));
        }

        [Fact]
        public void Can_find_approximate_subset_of_668_from_1000_random_elements()
        {
            var randomizer = new Random(10);
            var list = Enumerable.Range(0, 1000).Select(i => randomizer.Next(100)).ToList();
            float delta = 0.01f;
            List<int> result = new Scheduler().ApproximateSubsetFor(list, 668, delta);
            Assert.InRange(result.Sum(), 668 * (1 - delta), 668 * (1 + delta));
        }

    }
}
