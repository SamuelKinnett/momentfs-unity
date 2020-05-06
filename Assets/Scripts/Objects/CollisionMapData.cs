using MomenTFS.MAP.Collision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class CollisionMapData
    {
        private CollisionType[,] collisionMap;
        private Vector3 mapDimensions;

        public CollisionMapData(CollisionType[,] collisionMap, int blockHeight) {
            mapDimensions.x = collisionMap.GetLength(0);
            mapDimensions.z = collisionMap.GetLength(1);
            mapDimensions.y = blockHeight;

            this.collisionMap = new CollisionType[(int)mapDimensions.x, (int)mapDimensions.z];

            for (int x = 0; x < mapDimensions.x; ++x) {
                for (int z = 0; z < mapDimensions.z; ++z) {
                    this.collisionMap[x, (int)mapDimensions.z - 1 - z] = collisionMap[x, z];
                }
            }
        }

        public CollisionType GetBlock(int x, int y, int z) {
            if (x >= mapDimensions.x ||
                x < 0 ||
                z >= mapDimensions.z ||
                z < 0) {
                return CollisionType.SOLID;
            } else if (y >= mapDimensions.y ||
                       y < 0) {
                return CollisionType.EMPTY;
            }

            return collisionMap[x, z];
        }
    }
}
