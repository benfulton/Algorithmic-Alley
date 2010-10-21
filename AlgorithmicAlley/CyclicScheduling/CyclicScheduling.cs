using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace AlgorithmicAlley
{
    public class CyclicSchedule
    {
        List<Job> _jobs;

        public CyclicSchedule(List<Job> jobs)
        {
            _jobs = jobs;
        }

        IEnumerable<Job> Unblocked(int time)
        {
           return _jobs.Where( job => !job.Blocked( time ));
        }

        public void Process(int numIterations)
        {
            int time = 0;
            while (!IterationComplete(numIterations))
            {
                foreach (var job in Unblocked(time).Where(job => job.IsIdle(time)))
                {
                    job.Start(time);
                }

                time++;
            }
        }

        private bool IterationComplete(int i )
        {
            return !_jobs.Any( job => job.CompletedIterations() <= i);
        }

        public decimal CycleTime(int jobNum)
        {
            Contract.Requires( _jobs[jobNum].CompletedIterations() > 1);
            Contract.Requires(jobNum < _jobs.Count);

            var starts = _jobs[jobNum].StartTimes.ToList();

            return (decimal)Enumerable.Range(1, starts.Count - 1)
                .Select(i => starts[i] - starts[i - 1] )
                .Average();
        }

        public decimal IterationCompletionTime(int i)
        {
            var values = _jobs.Select(x => new { job = x, starttime = x.StartTime(i), endtime = x.StartTime(i) + x.processingTime });
            return values.Max(val => val.endtime) - values.Min(val => val.starttime);
        }

        public bool IsPeriodic()
        {
            return _jobs.All(job =>
                {
                    int period = job.StartTime(1) - job.StartTime(0);
                    for (int k = 1; k < job.CompletedIterations(); k++)
                    {
                        if (job.StartTime(k) - job.StartTime(k - 1) != period)
                            return false;
                    }

                    return true;
                });
        }

        public int Height(List<Job> path)
        {
            int result = 0;
            for (int i = 0; i < path.Count-1; i++)
            {
                result += path[i].Height(path[i + 1]);
            }
            return result;
        }
    }
}