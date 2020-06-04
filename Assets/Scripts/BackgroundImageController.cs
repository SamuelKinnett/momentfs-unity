using Assets.Scripts.Objects;
using System;
using System.Linq;
using UnityEngine;

public class BackgroundImageController : MonoBehaviour
{
    public GameObject Quad;

    private bool textureChanged = false;
    private bool isAnimated = false;
    private float timeUntilNextTextureChange;
    private int currentIndex;
    private Texture2D currentTexture;

    private BackgroundImageData data;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (this.isAnimated) {
            timeUntilNextTextureChange -= Time.deltaTime;

            if (timeUntilNextTextureChange <= 0) {
                currentIndex = (currentIndex + 1) % 8;

                currentTexture = data.Textures[currentIndex];
                timeUntilNextTextureChange = data.Durations[currentIndex];
                this.transform.position = data.Positions[currentIndex];
                textureChanged = true;
            }
        }

        if (textureChanged) {
            if (currentTexture != null) {
                Quad.GetComponent<Renderer>().material.mainTexture = currentTexture;
                Quad.GetComponent<MeshRenderer>().enabled = true;

                this.transform.localScale
                    = new Vector3(
                        currentTexture.width / data.mapDimensions.x,
                        currentTexture.height / data.mapDimensions.y, 
                        1.0f);
            } else {
                Quad.GetComponent<MeshRenderer>().enabled = false;
            }

            textureChanged = false;
        }
    }

    public void Initialise(BackgroundImageData data) {
        this.data = data;

        this.isAnimated = data.Durations.Count(d => d > 0) > 1;
        currentIndex = 0;

        timeUntilNextTextureChange = data.Durations[currentIndex];

        this.currentTexture = data.Textures[currentIndex];
        this.textureChanged = true;
        this.transform.position = data.Positions[currentIndex];
    }
}
