using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace AlgorithmicAlley
{
    public class Job
    {
        public int processingTime;
    }

    public class CyclicSchedule
    {
        List<Job> _jobs;
        Dictionary<Job, List<int>> _startingTimes;

        public CyclicSchedule(List<Job> jobs)
        {
            _jobs = jobs;
            _startingTimes = _jobs.ToDictionary(j => j, j => new List<int>());
        }

        public void Process(int numIterations)
        {
            // each job starts at 0
            _jobs.ForEach( job => _startingTimes[job].Add(0));

            for (int i = 1; i < numIterations; ++i)
            {
                _jobs.ForEach(job => _startingTimes[job].Add(_startingTimes[job].Last() + job.processingTime));
            }
        }

        public int StartTime(Job job, int iteration)
        {
            return _startingTimes[job][iteration];
        }

        public decimal CycleTime(int jobNum)
        {
            Contract.Requires(_startingTimes[_jobs[jobNum]].Count > 1);
            Contract.Requires(jobNum < _jobs.Count);

            var starts = _startingTimes[_jobs[jobNum]];

            return (decimal)Enumerable.Range(1, starts.Count - 1)
                .Select(i => starts[i] - starts[i - 1] )
                .Average();
        }

        public decimal IterationCompletionTime(int i)
        {
            var values = _startingTimes.Select(x => new { job = x.Key, starttime = x.Value[i], endtime = x.Value[i] + x.Key.processingTime });
            return values.Max(val => val.endtime) - values.Min(val => val.starttime);
        }
    }
}