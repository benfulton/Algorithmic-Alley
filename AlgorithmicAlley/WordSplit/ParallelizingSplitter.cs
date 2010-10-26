using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace AlgorithmicAlley
{
    public abstract class ParallelizingSplitter : Splitter
    {
        public static IEnumerable<string> MaybeWord(string str)
        {
            if (string.IsNullOrEmpty(str))
                yield break;
            else
                yield return str;
        }

        protected IWordState ProcessChar(char c)
        {
            if (c == ' ')
                return new Segment();
            else
                return new Chunk(c.ToString());
        }

        protected ParallelQuery<ParallelizingSplitter.IWordState> Map(string s)
        {
            return s.AsEnumerable().AsParallel().AsOrdered().Select(c => ProcessChar(c));
        }

        public interface IWordState
        {
            IWordState Combine(IWordState other);
            IEnumerable<string> Finalize();
        }

        protected class Chunk : IWordState
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
                    return new Chunk(this, other as Chunk);
                else
                {
                    var seg = other as Segment;

                    return new Segment(str + seg.Left, seg.Middle, seg.Right);
                }

            }

            public IEnumerable<string> Finalize()
            {
                if (string.IsNullOrEmpty(str))
                    yield break;
                else
                    yield return str;
            }

            public override string ToString()
            {
                return str;
            }
        }

        protected class Segment : IWordState
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

            public IWordState Combine(IWordState other)
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

            public override string ToString()
            {
                return Left + ", <" + string.Join(",", Middle) + ">, " + Right;
            }

        }
    }
}
