using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace AlgorithmicAlley
{
    public abstract class Splitter
    {
        public abstract IEnumerable<string> Split(string stringToSplit);
    }

    public class NormalSplitter : Splitter
    {
        public override IEnumerable<string> Split(string ss)
        {
            var result = new List<string>();
            string word = "";
            for (int i = 0; i < ss.Length; ++i)
            {
                if (ss[i] == ' ')
                {
                    if (!string.IsNullOrEmpty(word))
                    {
                        result.Add(word);
                        word = "";
                    }
                }
                else
                    word += ss[i];
            }
            if (!string.IsNullOrEmpty(word))
                result.Add(word);

            return result;
        }
    }


    public class AggregatingSplitter : Splitter
    {
        public override IEnumerable<string> Split(string ss)
        {
            string word = "";
            var words = ss.AsEnumerable().Aggregate(new List<string>(), (List<string> result, char c) =>
            {
                if (c == ' ')
                {
                    if (!string.IsNullOrEmpty(word))
                    {
                        result.Add(word);
                        word = "";
                    }
                }
                else
                    word += c;

                return result;
            }).ToList();

            if (!string.IsNullOrEmpty(word))
                words.Add(word);

            return words;
        }

    }

    public class ParallelSplitter : Splitter
    {
        private const int INT_PARALLEL_CHUNKS = 100;
        public override IEnumerable<string> Split(string s)
        {
            var x = s.AsEnumerable().AsParallel().AsOrdered().Select(c => ProcessChar(c));

            var subsets = Reduce(x).ToList();
            while (subsets.Count > INT_PARALLEL_CHUNKS)
            {
                subsets = Reduce(subsets).ToList();
            }

            if (!subsets.Any())
                return new List<string>();

            var final = subsets.Aggregate((runningProduct, nextFactor) => runningProduct.Combine(nextFactor));

            if (final is Chunk)
                return new List<string>{(final as Chunk).str};

            return (final as Segment).Finalize();
        }

        public IEnumerable<IWordState> Reduce(IEnumerable<IWordState> set)
        {
            var groups = set.Select((ws, index) => new { ws, index })
                .GroupBy(g => g.index / INT_PARALLEL_CHUNKS, i => i.ws);

            return groups.AsParallel().AsOrdered()
                .Select(batch => batch.Aggregate((runningProduct, nextFactor) => runningProduct.Combine(nextFactor)));

        }


        public static IEnumerable<string> MaybeWord(string str)
        {
            if (string.IsNullOrEmpty(str))
                yield break;
            else
                yield return str;
//                return new List<string>();
//            return new List<string> { s };
        }

        IWordState ProcessChar(char c)
        {
            if (c == ' ')
                return new Segment();
            else
                return new Chunk(c.ToString());
        }

        public interface IWordState
        {
            IWordState Combine( IWordState other);
        }

        class Chunk : IWordState
        {
            public readonly string str;

            public Chunk(string s)
            {
                str = s;
            }

            public Chunk(Chunk c1, Chunk c2)
            {
                str = c1.str + c2.str;
            }

            public IWordState Combine(IWordState other)
            {
                if (other is Chunk)
                    return new Chunk( this, other as Chunk);
                else
                {
                    var seg = other as Segment;

                    return new Segment(str + seg.Left, seg.Middle, seg.Right);
                }
            }
        }

        class Segment : IWordState
        {
            public readonly string Left;
            public readonly IEnumerable<string> Middle;
            public readonly string Right;

            public Segment()
            {
                Left = Right = "";
                Middle = new List<string>();
            }

            public Segment(string l, IEnumerable<string> m, string r)
            {
                Left = l;
                Middle = m;
                Right = r;
            }

            public IWordState Combine( IWordState other)
            {
                if (other is Chunk)
                    return new Segment(Left, Middle, Right + (other as Chunk).str);
                else
                {
                    var seg = other as Segment;
                    return new Segment(Left, 
                        Middle.Union(MaybeWord(Right + seg.Left).Union(seg.Middle)), seg.Right);
                }
            }

            public IEnumerable<string> Finalize()
            {
                if (!string.IsNullOrEmpty(Left))
                    yield return Left;

                foreach (var str in Middle)
                    yield return str;

                if (!string.IsNullOrEmpty(Right))
                    yield return Right;
            }

        }
    }
}
