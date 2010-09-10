using System;
using System.Linq;
using System.Collections.Generic;

namespace AlgorithmicAlley
{
    public class Job
    {
        public int processingTime;
        List<int> startingTimes;
        List<UniformConstraint> Blockers;

        public Job()
        {
            startingTimes = new List<int>();
            Blockers = new List<UniformConstraint>();
        }

        public IEnumerable<int> StartTimes
        {
            get
            {
                return startingTimes;
            }
        }

        public void Start(int time)
        {
            startingTimes.Add(time);
        }

        public bool IsIdle(int time)
        {
            return !StartTimes.Any() || StartTimes.Last() + processingTime <= time;
        }

        public void Update()
        {
            int startTime = StartTimes.Any() ? StartTimes.Last() + processingTime : 0;
            Start(startTime);
        }

        public int StartTime(int iteration)
        {
            return startingTimes[iteration];
        }

        public void Constrain(UniformConstraint constraint)
        {
            Blockers.Add(constraint);
        }

        public bool Blocked(int time)
        {
            return Blockers.Any(constraint => constraint.Blocker(startingTimes.Count, time));
        }

        public int CompletedIterations()
        {
            return startingTimes.Count;
        }

    }
}
