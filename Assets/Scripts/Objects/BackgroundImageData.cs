using MomenTFS.MAP.Objects;
using MomenTFS.TIM;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class BackgroundImageData
    {
        public Texture2D[] Textures;
        public Vector3[] Positions;
        public float[] Durations;
        public Vector2 mapDimensions;

        public BackgroundImageData(
                TIMImage[] timImages,
                MAPObject[] mapObjects,
                MAPObjectInstance mapObjectInstance,
                Vector2 mapDimensions) {
            Textures = new Texture2D[8];
            Positions = new Vector3[8];
            Durations = new float[8];
            this.mapDimensions = mapDimensions;

            var aspectRatio = mapDimensions.x / mapDimensions.y;

            for (var i = 0; i < mapObjectInstance.AnimationState.Length; ++i) {
                var objectIndex = mapObjectInstance.AnimationState[i];

                if (objectIndex < 0) {
                    Textures[i] = null;
                    Positions[i] = Vector3.zero;
                    Durations[i] = mapObjectInstance.AnimationDuration[i] * 0.06f;
                } else {
                    MAPObject mapObject = mapObjects[objectIndex];
                    if (mapObject.Z > 0) {
                        Debug.Log(mapObject.Z);
                    }
                    Textures[i] = getTextureFromTIMImages(timImages, mapObject);
                    //Positions[i] = new Vector3(
                    //    (-0.5f + (mapObjectInstance.X / mapDimensions.x)) * aspectRatio,
                    //    0.5f - (mapObjectInstance.Y / mapDimensions.y),
                    //    mapObject.Z - 0.5f);
                    Positions[i] = new Vector3(
                        (-0.5f + (mapObjectInstance.X / mapDimensions.x)) * aspectRatio,
                        0.5f - (mapObjectInstance.Y / mapDimensions.y),
                        50.0f - (mapObject.Z / 10000f));
                    Durations[i] = mapObjectInstance.AnimationDuration[i] * 0.06f;
                }
            }
        }

        private Texture2D getTextureFromTIMImages(TIMImage[] images, MAPObject mapObject) {
            Texture2D newTexture = new Texture2D(
                mapObject.Width,
                mapObject.Height,
                TextureFormat.RGBA32,
                false);
            var imageIndex = (int)Math.Floor(mapObject.SpritesheetX / 256f);
            if (imageIndex >= images.Length) {
                return null;
            }

            TIMImage image = images[imageIndex];
            System.Drawing.Color[,] imageData = image.GetBitmap();
            List<Color> colorData = new List<Color>();

            int imageX = mapObject.SpritesheetX % 256;
            int imageY = mapObject.SpritesheetY % 256;

            for (int y = imageY + mapObject.Height - 1; y >= imageY; --y) {
                for (int x = imageX; x < imageX + mapObject.Width; ++x) {
                    System.Drawing.Color currentColor = imageData[x, y];
                    colorData.Add(
                        new Color(
                            currentColor.R / 255f,
                            currentColor.G / 255f,
                            currentColor.B / 255f)); 
                }
            }

            newTexture.SetPixels(colorData.ToArray());
            newTexture.filterMode = FilterMode.Point;
            newTexture.Apply();

            return newTexture;
        }
    }
}
