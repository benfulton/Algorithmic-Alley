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
        public SubsetSumTests()
        {
            var randomizer = new Random(10);
            ListOf35 = Enumerable.Range(0, 35).Select(i => randomizer.Next(100)).ToList();
            ListOf100 = Enumerable.Range(0, 100).Select(i => randomizer.Next(200, 5000)).ToList();

        }

        Scheduler CreateScheduler( SubsetSumStrategy strategy )
        {
            return new Scheduler(strategy);
        }

        List<int> ListOf10 = new List<int> { 1, 2, 92, 437, 16, 46, 991, 12, 92, 130 };
        List<int> ListOf35;
        List<int> ListOf100;

        [Fact]
        public void A_subset_of_one_and_two_sums_to_one()
        {
            var list = new List<int> { 1, 2 };
            var scheduler = CreateScheduler(new EnumerateAllSubsets());
            Assert.Equal( 2,  scheduler.FindJobsWithTotalLength(list, 2).Sum());
        }

        [Fact]
        public void Can_find_subset_of_14_from_5_elements()
        {
            var list = new List<int> { 1, 2,3,4,10 };
            var scheduler = CreateScheduler(new EnumerateAllSubsets());
            Assert.Equal(14, scheduler.FindJobsWithTotalLength(list, 14).Sum());
        }

        [Fact]
        public void Can_find_subset_of_668_from_10_elements()
        {
            var scheduler = CreateScheduler(new EnumerateAllSubsets());
            var result = scheduler.FindJobsWithTotalLength(ListOf10, 668);
            Assert.Equal(668, result.Sum());
        }

        [Fact]
        public void Greedy_does_not_find_exact_subset_of_668_from_10_elements()
        {
            var scheduler = CreateScheduler(new Greedy());
            var result = scheduler.FindJobsWithTotalLength(ListOf10, 668);
            Assert.NotEqual(result.Sum(), 668);
        }

        [Fact]
        public void Greedy_can_find_approximate_subset_of_668_from_10_elements()
        {
            float delta = 0.01f;
            var scheduler = CreateScheduler(new Greedy());
            var result = scheduler.FindJobsWithTotalLength(ListOf10, 668);
            Assert.InRange(result.Sum(), 668 * (1 - delta), 668 * (1 + delta));
        }


        [Fact]
        public void Can_find_approximate_subset_of_668_from_10_elements()
        {
            float delta = 0.001f;
            var scheduler = CreateScheduler(new ApproximatingSubsets(delta));
            List<int> result = scheduler.FindJobsWithTotalLength(ListOf10, 668);
            Assert.NotNull(result);
            Assert.InRange(result.Sum(), 668 * (1 - delta), 668 * (1 + delta));
        }

        [Fact]
        public void Cannot_enumerate_35_jobs_and_get_result()
        {
            var scheduler = CreateScheduler(new EnumerateAllSubsets());
            var result = scheduler.FindJobsWithTotalLength(ListOf35, 668);
            Assert.Null(result);
        }

        [Fact]
        public void Greedy_can_find_approximate_subset_of_668_from_35_random_elements()
        {
            float delta = 0.002f;
            var scheduler = CreateScheduler(new Greedy());
            var result = scheduler.FindJobsWithTotalLength(ListOf35, 668);
            Assert.InRange(result.Sum(), 668 * (1 - delta), 668 * (1 + delta));
        }

        [Fact]
        public void Expander_can_find_subset_of_668_from_35_random_elements()
        {
            float delta = 0.0001f;
            var scheduler = CreateScheduler(new ExpandSubsets());
            List<int> result = scheduler.FindJobsWithTotalLength(ListOf35, 668);
            Assert.InRange(result.Sum(), 668 * (1 - delta), 668 * (1 + delta));
        }

        [Fact]
        public void Can_find_approximate_subset_of_668_from_35_random_elements()
        {
            float delta = 0.0001f;
            var scheduler = CreateScheduler(new ApproximatingSubsets(delta));
            List<int> result = scheduler.FindJobsWithTotalLength(ListOf35, 668);
            Assert.InRange(result.Sum(), 668 * (1 - delta), 668 * (1 + delta));
        }

        [Fact]
        public void Greedy_can_find_approximate_subset_of_6687_from_100_random_elements()
        {
            float delta = 0.01f;
            var scheduler = CreateScheduler(new Greedy());
            var result = scheduler.FindJobsWithTotalLength(ListOf100, 6687);
            Assert.InRange(result.Sum(), 6680 * (1 - delta), 6687 * (1 + delta));
        }

        [Fact]
        public void Can_find_approximate_subset_of_6687_from_100_random_elements()
        {
            float delta = 0.00001f;
            var scheduler = CreateScheduler(new ApproximatingSubsets(delta));
            List<int> result = scheduler.FindJobsWithTotalLength(ListOf100, 6687);
            Assert.InRange(result.Sum(), 668 * (1 - delta), 6687 * (1 + delta));
        }

    }
}
