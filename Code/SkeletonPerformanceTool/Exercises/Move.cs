using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Samples.Kinect.SkeletonBasics.Exercises
{

    public enum Moves
    {
        rest,
        warmUp,
        coolDown,
        jumpingJack,
        burpee,
        highJump
    };
    class Move
    {
        public string Name;
        public Moves Type;
        private int Count = 0;
        private int Duration;

        public Move(int type, int duration)
        {
            Type = (Moves)type;
            Name = Type.ToString();
            Duration = duration;
        }

        public int GetCount()
        {
            return Count;
        }

        public void IncrementCount()
        {
            Count++;
        }

        public void SetCount(int count)
        {
            Count = count;
        }

        public float GetRate()
        {
            return (float)Count / (float)Duration;
        }
    }
    
}
