using Assets.Scripts.Objects;
using MomenTFS.MAP.Collision;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class CollisionMeshController : MonoBehaviour
    {
        public MeshFilter MeshFilter;
        public MeshCollider MeshCollider;

        private List<Vector3> newVertices;
        private List<int> newTriangles;
        private List<Vector2> newUV;

        private int Width;
        private int Height;
        private int Length;
        private int faceCount;
        private float scale;

        private Mesh mesh;

        public void Initialise(
                int width,
                int height,
                int length,
                float scale) {
            Width = width;
            Height = height;
            Length = length;
            this.scale = scale;

            mesh = MeshFilter.mesh;

            newVertices = new List<Vector3>();
            newTriangles = new List<int>();
            newUV = new List<Vector2>();
        }

        public void OnDrawGizmos() {
            Gizmos.DrawSphere(Vector3.zero, 1.0f);
            Gizmos.DrawSphere(new Vector3((Width - 1) * scale, 0, (Length - 1) * scale), 1.0f);
        }

        public void GenerateMesh(CollisionMapData mapData) {
            for (int x = 0; x < Width; ++x) {
                for (int y = 0; y < Height; ++y) {
                    for (int z = 0; z < Length; ++z) {
                        CollisionType curBlock = mapData.GetBlock(x, y, z);

                        // If this block is solid
                        if (curBlock == CollisionType.SOLID) {
                            // Compare the block to all of its neighbours. Any 
                            // side that faces air should have a face rendered.
                            float scaledX = x * scale;
                            float scaledY = y * scale;
                            float scaledZ = z * scale;

                            if (mapData.GetBlock(x, y + 1, z) == CollisionType.EMPTY) {
                                CreateCubeTopFace(scaledX, scaledY, scaledZ);
                            }

                            if (mapData.GetBlock(x, y, z + 1) == CollisionType.EMPTY) {
                                CreateCubeNorthFace(scaledX, scaledY, scaledZ);
                            }

                            if (mapData.GetBlock(x + 1, y, z) == CollisionType.EMPTY) {
                                CreateCubeEastFace(scaledX, scaledY, scaledZ);
                            }

                            if (mapData.GetBlock(x, y, z - 1) == CollisionType.EMPTY) {
                                CreateCubeSouthFace(scaledX, scaledY, scaledZ);
                            }

                            if (mapData.GetBlock(x - 1, y, z) == CollisionType.EMPTY) {
                                CreateCubeWestFace(scaledX, scaledY, scaledZ);
                            }

                            if (mapData.GetBlock(x, y - 1, z) == CollisionType.EMPTY) {
                                CreateCubeBottomFace(scaledX, scaledY, scaledZ);
                            }
                        }
                    }
                }
            }

            UpdateMesh();
        }

        private void Update() {
            //if (ChunkUpdated) {
            //    GenerateMesh(0.0f, false);
            //    ChunkUpdated = false;
            //}

            //if (currentTimeUntilRegenerateMesh > 0) {
            //    currentTimeUntilRegenerateMesh = currentTimeUntilRegenerateMesh - Time.deltaTime;

            //    MeshRendererPreview.material.color = new Color(1, 1, 1, 1 - (currentTimeUntilRegenerateMesh / timeUntilRegenerateMesh));

            //    if (currentTimeUntilRegenerateMesh <= 0) {
            //        currentTimeUntilRegenerateMesh = 0;
            //        GenerateMesh(0.0f, false);
            //    }
            //}
        }

        private void UpdateMesh() {
            // Clear the mesh and set the vertices, UVs and triangles
            mesh.Clear();
            mesh.SetVertices(newVertices);
            mesh.SetUVs(0, newUV);
            mesh.SetTriangles(newTriangles, 0);

            // Recalculate normals
            mesh.RecalculateNormals();

            // Create the collision mesh
            MeshCollider.sharedMesh = mesh;

            // Clear the buffers
            newVertices.Clear();
            newUV.Clear();
            newTriangles.Clear();

            faceCount = 0;
        }

        private void CreateCubeTopFace(float x, float y, float z) {
            // Add the new vertices
            newVertices.Add(new Vector3(x, y, z));
            newVertices.Add(new Vector3(x, y, z + scale));
            newVertices.Add(new Vector3(x + scale, y, z + scale));
            newVertices.Add(new Vector3(x + scale, y, z));

            ApplyTextureToFace();
        }

        private void CreateCubeNorthFace(float x, float y, float z) {
            // Add the new vertices
            newVertices.Add(new Vector3(x + scale, y - scale, z + scale));
            newVertices.Add(new Vector3(x + scale, y, z + scale));
            newVertices.Add(new Vector3(x, y, z + scale));
            newVertices.Add(new Vector3(x, y - scale, z + scale));

            ApplyTextureToFace();
        }

        private void CreateCubeEastFace(float x, float y, float z) {
            // Add the new vertices
            newVertices.Add(new Vector3(x + scale, y - scale, z));
            newVertices.Add(new Vector3(x + scale, y, z));
            newVertices.Add(new Vector3(x + scale, y, z + scale));
            newVertices.Add(new Vector3(x + scale, y - scale, z + scale));

            ApplyTextureToFace();
        }

        private void CreateCubeSouthFace(float x, float y, float z) {
            // Add the new vertices
            newVertices.Add(new Vector3(x, y - scale, z));
            newVertices.Add(new Vector3(x, y, z));
            newVertices.Add(new Vector3(x + scale, y, z));
            newVertices.Add(new Vector3(x + scale, y - scale, z));

            ApplyTextureToFace();
        }

        private void CreateCubeWestFace(float x, float y, float z) {
            // Add the new vertices
            newVertices.Add(new Vector3(x, y - scale, z + scale));
            newVertices.Add(new Vector3(x, y, z + scale));
            newVertices.Add(new Vector3(x, y, z));
            newVertices.Add(new Vector3(x, y - scale, z));

            ApplyTextureToFace();
        }

        private void CreateCubeBottomFace(float x, float y, float z) {
            // Add the new vertices
            newVertices.Add(new Vector3(x, y - scale, z));
            newVertices.Add(new Vector3(x + scale, y - scale, z));
            newVertices.Add(new Vector3(x + scale, y - scale, z + scale));
            newVertices.Add(new Vector3(x, y - scale, z + scale));

            ApplyTextureToFace();
        }

        private void ApplyTextureToFace() {
            // Generate the triangles
            var offset = faceCount * 4;

            newTriangles.Add(offset);       // 1
            newTriangles.Add(offset + 1);   // 2
            newTriangles.Add(offset + 2);   // 3
            newTriangles.Add(offset);
            newTriangles.Add(offset + 2);
            newTriangles.Add(offset + 3);

            newUV.Add(new Vector2(0, 0));
            newUV.Add(new Vector2(0, 1));
            newUV.Add(new Vector2(1, 1));
            newUV.Add(new Vector2(1, 0));

            ++faceCount;
        }
    }
}
