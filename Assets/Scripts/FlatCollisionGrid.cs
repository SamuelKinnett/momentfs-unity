using MomenTFS.MAP.Collision;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class FlatCollisionGrid : MonoBehaviour
    {
        public MeshFilter MeshFilter;
        public Vector2 textureMapDimensions;

        private List<Vector3> newVertices;
        private List<int> newTriangles;
        private List<Vector2> newUV;

        private int faceCount;

        private float textureUnit = 0.25f;

        public void Generate(float xPosition, float yPosition, float scale, CollisionType[,] collisionMap) {
            newVertices = new List<Vector3>();
            newTriangles = new List<int>();
            newUV = new List<Vector2>();
            faceCount = 0;

            for (int x = 0; x < collisionMap.GetLength(0); ++x) {
                for (int y = 0; y < collisionMap.GetLength(1); ++y) {
                    CollisionType collisionType = collisionMap[x, collisionMap.GetLength(1) - 1 - y];

                    CreateTile(xPosition + x * scale, yPosition + y * scale, scale, collisionType);
                }
            }

            Mesh mesh = MeshFilter.mesh;

            // Clear the mesh and set the vertices, UVs and triangles
            mesh.Clear();
            mesh.SetVertices(newVertices);
            mesh.SetUVs(0, newUV);
            mesh.SetTriangles(newTriangles, 0);

            // Recalculate normals
            mesh.RecalculateNormals();

            // Create the collision mesh
            //MeshCollider.sharedMesh = mesh;

            // Clear the buffers
            newVertices.Clear();
            newUV.Clear();
            newTriangles.Clear();

            faceCount = 0;
        }

        private void CreateTile(float x, float z, float scale, CollisionType collisionType) {
            // Add the new vertices
            newVertices.Add(new Vector3(x, 0, z));
            newVertices.Add(new Vector3(x, 0, z + scale));
            newVertices.Add(new Vector3(x + scale, 0, z + scale));
            newVertices.Add(new Vector3(x + scale, 0, z));

            ApplyTextureToFace(collisionType);
        }

        private void ApplyTextureToFace(CollisionType collisionType) {
            var texturePosition = ConvertCollisionTypeToTexturePosition(collisionType);

            // Generate the triangles
            var offset = faceCount * 4;

            newTriangles.Add(offset);       // 1
            newTriangles.Add(offset + 1);   // 2
            newTriangles.Add(offset + 2);   // 3
            newTriangles.Add(offset);
            newTriangles.Add(offset + 2);
            newTriangles.Add(offset + 3);

            var xOrigin = textureUnit * texturePosition.x;
            var yOrigin = textureUnit * texturePosition.y;

            newUV.Add(new Vector2(xOrigin, yOrigin));
            newUV.Add(new Vector2(xOrigin, yOrigin + textureUnit));
            newUV.Add(new Vector2(xOrigin + textureUnit, yOrigin + textureUnit));
            newUV.Add(new Vector2(xOrigin + textureUnit, yOrigin));

            ++faceCount;
        }

        // Given a texture index, convert it to the corresponding texture
        // position. Texture indices go from left to right, top to bottom.
        private Vector2 ConvertCollisionTypeToTexturePosition(CollisionType collisionType) {
            int textureIndex;

            switch (collisionType) {
                case CollisionType.EMPTY:
                    textureIndex = 0;
                    break;
                case CollisionType.SOLID:
                    textureIndex = 1;
                    break;
                case CollisionType.EXIT_1:
                case CollisionType.EXIT_2:
                case CollisionType.EXIT_3:
                case CollisionType.EXIT_4:
                case CollisionType.EXIT_5:
                case CollisionType.EXIT_6:
                case CollisionType.EXIT_7:
                case CollisionType.EXIT_8:
                case CollisionType.EXIT_9:
                case CollisionType.EXIT_10:
                    textureIndex = 2 + (int)collisionType - 110;
                    break;
                case CollisionType.TOILET:
                    textureIndex = 12;
                    break;
                default:
                    textureIndex = 13;
                    break;
            }

            var maxIndex = (int)textureMapDimensions.x * (int)textureMapDimensions.y - 1;
            var newIndex = Mathf.Clamp(textureIndex, 0, maxIndex);

            var textureX = newIndex % (int)textureMapDimensions.x;
            var textureY = (textureMapDimensions.y - 1) - (newIndex / (int)textureMapDimensions.y);

            return new Vector2(textureX, textureY);
        }

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}
