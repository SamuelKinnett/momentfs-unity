using Assets.Scripts.Objects;
using MomenTFS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private RoomData roomData;
    private Texture2D background;
    private List<GameObject> backgroundImages;

    public GameObject backgroundImagePrefab;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        if (roomData != null) {
            for (var i = 0; i < roomData.MAPData.Objects.InstanceCount; ++i) {
                var currentObject = roomData.MAPData.Objects.Instances[i];
                var animationState = currentObject.AnimationState[0];
                if (animationState > -1) {
                    Gizmos.DrawSphere(new Vector3(
                        (-0.5f + (currentObject.X / (float)roomData.TFSData.ImageSize.X))
                            * this.transform.localScale.x,
                        0.5f - (currentObject.Y / (float)roomData.TFSData.ImageSize.Y),
                        roomData.MAPData.Objects.Objects[animationState].Z),
                        0.01f);
                }
            }
        }
    }

    public void Initialise(RoomData roomData) {
        this.roomData = roomData;

        background = new Texture2D(
            roomData.TFSData.ImageSize.X,
            roomData.TFSData.ImageSize.Y,
            TextureFormat.RGBA32,
            false);

        System.Drawing.Color[,] tfsImageData = roomData.TFSData.GetBitmap(0);
        List<Color> colorData = new List<Color>();

        for (int y = tfsImageData.GetLength(1) - 1; y >= 0; --y) {
            for (int x = 0; x < tfsImageData.GetLength(0); ++x) {
                System.Drawing.Color currentColor = tfsImageData[x, y];
                colorData.Add(
                    new Color(
                        currentColor.R / 255f,
                        currentColor.G / 255f,
                        currentColor.B / 255f));
            }
        }

        background.SetPixels(colorData.ToArray());
        background.filterMode = FilterMode.Point;
        background.Apply();

        this.GetComponent<Renderer>().material.mainTexture = background;
        this.transform.localScale = new Vector3(
            roomData.TFSData.ImageSize.X / (float)roomData.TFSData.ImageSize.Y, 1, 1);

        backgroundImages = new List<GameObject>();
        Vector2 mapDimensions
            = new Vector2(roomData.TFSData.ImageSize.X, (float)roomData.TFSData.ImageSize.Y);

        for (int i = 0; i < roomData.MAPData.Objects.Instances.Length; ++i) {
            BackgroundImageData data = new BackgroundImageData(
                roomData.MAPData.TIMImages.ToArray(),
                roomData.MAPData.Objects.Objects,
                roomData.MAPData.Objects.Instances[i],
                mapDimensions);

            GameObject newBackgroundImage = Instantiate(backgroundImagePrefab);
            newBackgroundImage.transform.parent = this.transform;
            newBackgroundImage.GetComponent<BackgroundImageController>().Initialise(data);

            backgroundImages.Add(newBackgroundImage);
        }
    }

    private static Color SystemColorToUnityColor(System.Drawing.Color color) {
        return new Color(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
    }
}
