using System;
using Xunit;
using System.IO;
using System.Diagnostics;
using AlgorithmicAlley;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmicAlleyTests
{
    class SplitterTests
    {
        [Fact]
        public void SimpleSplitterTests()
        {
            var splitter = new NormalSplitter();
            Check(splitter);     
        }

        [Fact]
        public void AggregatingSplitterTests()
        {
            var splitter = new AggregatingSplitter();
            Check(splitter);
        }
        
        [Fact]
        public void ParallelSplitterTests()
        {
            var splitter = new GroupingSplitter();
            Check(splitter);
        }

        [Fact]
        public void DivideAndConquerSplitterTests()
        {
            var splitter = new DivideAndConquerSplitter();
            Check(splitter);
        }

        void Check(Splitter splitter)
        {
            Assert.Equal(new List<string> { "This", "is", "a", "sample" }, splitter.Split("This is a sample").ToList());
            Assert.Equal(new List<string> { "Here", "is", "another", "sample" }, splitter.Split(" Here is another sample ").ToList());
            Assert.Equal(new List<string> { "JustOneWord" }, splitter.Split("JustOneWord").ToList());
            Assert.Empty(splitter.Split(" "));
            Assert.Empty(splitter.Split(""));

            Assert.Equal(new List<string> { "Here", "is", "a", "sesquipedalian", "string", "of", "words" }, splitter.Split("Here is a sesquipedalian string of words").ToList());
        }

        void SplitHugeString(Splitter splitter)
        {
            StreamReader SR;

            SR = File.OpenText(@"MobyDick.txt");
            string text = SR.ReadToEnd();

            var w = Stopwatch.StartNew();
            var result = splitter.Split(text);
            w.Stop();
            Console.WriteLine(String.Format("ms {0}: ", splitter.GetType()) + w.ElapsedMilliseconds);
        }

        [Fact]
        public void CanSplitDivideAndConquer()
        {
            var splitter = new DivideAndConquerSplitter();
            //SplitHugeString(splitter);
        }

        [Fact]
        public void CanSplitGrouping()
        {
            var splitter = new GroupingSplitter();
            SplitHugeString(splitter);
        }

        [Fact]
        public void CanSplitAggregating()
        {
            var splitter = new AggregatingSplitter();
            SplitHugeString(splitter);
        }

        [Fact]
        public void CanSplit()
        {
            var splitter = new NormalSplitter();
            SplitHugeString(splitter);
        }
    }
}
