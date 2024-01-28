using MomenTFS.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class IVectorExtensions
    {
        public static Vector3 ToVector3(this IVector3 iVector) {
            float x = iVector.X;
            float y = iVector.Y;
            float z = iVector.Z;
            //Vector3 newVector = new Vector3(x, -z, y);
            Vector3 newVector = new Vector3(x, -y, z);



            Console.WriteLine(newVector);
            return newVector;
        }
    }
}
