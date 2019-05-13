using BulletSharp.Math;
using Microsoft.DirectX.DirectInput;
using System.Collections.Generic;

namespace CShark.Utils
{
    public static class Constants
    {
        public static IList<Key> MovementKeys = new List<Key>() { Key.W, Key.S, Key.A, Key.D };
        public static Vector3 StandardGravity = new Vector3(0, -300f, 0);
        public static Vector3 UnderWaterGravity = new Vector3(0,-20f,0);
    }
}
