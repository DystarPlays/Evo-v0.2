using Microsoft.Xna.Framework;
using System;
using System.Threading;

namespace Evo_v0._2
{
    public class Worker
    {
        public static class StaticRandom
        {
            private static int seed;

            private static ThreadLocal<Random> threadLocal = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

            static StaticRandom()
            {
                seed = Environment.TickCount;
            }

            public static Random Instance { get { return threadLocal.Value; } }
        }

        public struct Circle
        {
            public Vector2 Center { get; set; }
            public float Radius { get; set; }

            public Circle(Vector2 center, float radius)
            {
                Center = center;
                Radius = radius;
            }

            public bool Contains(Vector2 point)
            {
                return ((point - Center).Length() <= Radius);
            }

            public bool Intersects(Circle other)
            {
                return ((other.Center - Center).Length() < (other.Radius - Radius));
            }
        }
    }
}
