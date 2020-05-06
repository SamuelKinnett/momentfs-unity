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
            Vector3 newVector = new Vector3(iVector.X, -iVector.Z, iVector.Y);


            Console.WriteLine(newVector);
            return newVector;
        }
    }
}
