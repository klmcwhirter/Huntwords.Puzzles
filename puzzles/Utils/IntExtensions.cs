using System;

namespace puzzles.Utils
{
    public static class IntExtentions
    {
        static Random RandomInstance { get; set; } = new Random((int)DateTime.UtcNow.Ticks);

        public static int Random(this int i)
        {
            var rc = RandomInstance.Next(i);
            return rc;
        }
    }
}
