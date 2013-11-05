using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Samples.Kinect.SkeletonBasics.Exercises
{
    class Move
    {
        enum Moves
        {
            jumpingJacks,
            burpees,
            highJumps
        };

        string Name;
        Moves Type;

        public Move(int type)
        {
            Type = (Moves)type;
            Name = Type.ToString();
        }
    }
}
