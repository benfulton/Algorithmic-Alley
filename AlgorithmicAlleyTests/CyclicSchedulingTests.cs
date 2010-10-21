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
            Assert.Equal(0, _jobs.Last().StartTimes.First());
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

        [Fact]
        public void Constraint_blocks_job2_until_job1_completes()
        {
            _jobs[1].Constrain( new UniformConstraint( _jobs[0], 0, 0 ));
            _jobs[0].Start(0);
            Assert.True(_jobs[1].Blocked(2));
            Assert.False(_jobs[1].Blocked(7));
        }

        [Fact]
        public void Constraint_blocks_job2_until_job1_completes_n_iterations()
        {
            _jobs[1].Constrain(new UniformConstraint(_jobs[0], 0, 2));
            _jobs[0].Start(0);
            Assert.True(_jobs[1].Blocked(2));
            _jobs[0].Start(7);
            Assert.True(_jobs[1].Blocked( 10));
            _jobs[0].Start(14);
            Assert.False(_jobs[1].Blocked(22));
        }

        [Fact]
        public void Constrain_job2_to_start_after_job1()
        {
            _jobs[1].Constrain(new UniformConstraint(_jobs[0], 0, 0));
            var sched = new CyclicSchedule(_jobs);
            sched.Process(1);
            Assert.Equal(16m, sched.IterationCompletionTime(0));
        }

        [Fact]
        public void Constrain_job2_to_start_after_job1_with_latency()
        {
            _jobs[1].Constrain( new UniformConstraint(_jobs[0], 3, 0));
            var sched = new CyclicSchedule(_jobs);
            sched.Process(1);
            Assert.Equal(19m, sched.IterationCompletionTime(0));
        }

        [Fact]
        public void Constrain_job2_to_start_after_two_rounds_of_job1()
        {
            _jobs[1].Constrain(new UniformConstraint( _jobs[0], 0,2));
            var sched = new CyclicSchedule(_jobs);
            sched.Process(3);
            Assert.Equal(30m, sched.IterationCompletionTime(0));
        }

        [Fact]
        public void Complex_example()
        {
            _jobs = new List<Job>
            {
                new Job{ processingTime = 1 },
                new Job{ processingTime = 2 },
                new Job{ processingTime = 2 },
                new Job{ processingTime = 2 },
                new Job{ processingTime = 3 }
            };

            _jobs[2].Constrain(new UniformConstraint(_jobs[0], 0, 2));
            _jobs[4].Constrain(new UniformConstraint(_jobs[3], 0, 3));
            _jobs[1].Constrain(new UniformConstraint(_jobs[0], 0, 0));
            _jobs[2].Constrain(new UniformConstraint(_jobs[1], 0, 0));
            _jobs[3].Constrain(new UniformConstraint(_jobs[2], 6, 0));
            _jobs[4].Constrain(new UniformConstraint(_jobs[3], 0, 0));

            var sched = new CyclicSchedule(_jobs);
            sched.Process(100);
            Assert.Equal(22m, sched.IterationCompletionTime(0));
            Assert.Equal(24m, sched.IterationCompletionTime(1));
            Assert.Equal(26m, sched.IterationCompletionTime(2));

            Assert.True(sched.IsPeriodic());
        }

        [Fact]
        public void foo()
        {
        }
    }
}
