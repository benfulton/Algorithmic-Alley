using System;

namespace AlgorithmicAlley
{
    public class UniformConstraint
    {
        public readonly Job BlockingJob;

        public readonly int Latency;
        public readonly int Iterations;

        public UniformConstraint(Job blocker, int l, int h)
        {
            BlockingJob = blocker;
            Latency = l;
            Iterations = h;
        }

        public bool Blocker(int iteration, int time)
        {
            int triggerIteration = iteration + Iterations;
            if (triggerIteration >= BlockingJob.CompletedIterations())
                return true;

            return BlockingJob.StartTime(triggerIteration) + BlockingJob.processingTime + Latency > time;
        }


    }
}
