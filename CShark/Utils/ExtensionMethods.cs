using BulletSharp.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;

namespace CShark.Utils
{
    public static class ExtensionMethods
    {
        public static TGCVector3 ToTGCVector3(this Vector3 vector) {
            return new TGCVector3(vector.X, vector.Y, vector.Z);
        }
    }
}
