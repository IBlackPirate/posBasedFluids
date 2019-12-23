using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;

namespace PBF.Scripts.Data
{
    public static class GameData
    {
        public static bool EnableCollisions;

        public static float Gravity;
        public static float3 VectorDown => new float3(0, -1, 0);

        public static Bounds Bounds;

        public static float FluidRadius;
        public static float FluidDiameter => 2 * FluidRadius;

        private static Unity.Mathematics.Random random = new Unity.Mathematics.Random(2007);
        public static Unity.Mathematics.Random Random => random;
    }

    [Serializable]
    public struct Bounds
    {
        public float YBottomBound;
        public float YTopBound;
        public float XLeftBound;
        public float XRightBound;
        public float ZForwardBound;
        public float ZBackBound;
    }
}
