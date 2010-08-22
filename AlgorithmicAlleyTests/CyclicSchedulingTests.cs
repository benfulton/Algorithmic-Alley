using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using AlgorithmicAlley;

namespace AlgorithmicAlleyTests
{
    public class CyclicSchedulingTests
    {
        List<Job> _jobs;

        public CyclicSchedulingTests()
        {
            _jobs = new List<Job> { 
                new Job { processingTime = 7 },
                new Job { processingTime = 9 },
                new Job { processingTime = 2 }
            };

        }

        [Fact]
        public void CycleTime_for_one_job_is_processingTime()
        {
            var jobs = new List<Job> { new Job{ processingTime = 3 } };
            var sched = new CyclicSchedule(jobs);
            sched.Process(2);
            Assert.Equal(3, sched.CycleTime(0));
        }

        [Fact]
        public void Last_job_for_one_iteration_starts_at_0()
        {
            var sched = new CyclicSchedule(_jobs);
            sched.Process(1);
            Assert.Equal(0, sched.StartTime(_jobs.Last(), 0));
        }

        [Fact]
        public void Three_jobs_of_time_three_process_evenly()
        {
            _jobs = new List<Job> { 
                new Job { processingTime = 3 },
                new Job { processingTime = 3 },
                new Job { processingTime = 3 }
            };
            var sched = new CyclicSchedule(_jobs);
            sched.Process(3);
            Assert.Equal(3m, sched.IterationCompletionTime(0));
            Assert.Equal(3m, sched.IterationCompletionTime(1));
        }
        [Fact]
        public void CycleTime_for_one_job_five_iterations_is_processing_time()
        {
            var jobs = new List<Job> { new Job { processingTime = 3 } };
            var sched = new CyclicSchedule(jobs);
            sched.Process(5);
            Assert.Equal(3m, sched.CycleTime(0));
        }

        [Fact]
        public void IterationCompletionTime_is_last_ending_minus_first_starting()
        {
            var jobs = new List<Job> { new Job { processingTime = 7 }, new Job { processingTime = 9 }, new Job { processingTime = 2 } };
            var sched = new CyclicSchedule(jobs);
            sched.Process(5);
            Assert.Equal(9m, sched.IterationCompletionTime(0));
            Assert.Equal(16m, sched.IterationCompletionTime(1));
            Assert.Equal(23m, sched.IterationCompletionTime(2));
            Assert.Equal(30m, sched.IterationCompletionTime(3));
        }
    }
}
