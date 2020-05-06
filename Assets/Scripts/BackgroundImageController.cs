using Assets.Scripts.Objects;
using UnityEngine;

public class BackgroundImageController : MonoBehaviour
{
    public GameObject Quad;

    private bool textureChanged = false;
    private Texture2D currentTexture;

    private BackgroundImageData data;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (textureChanged) {
            if (currentTexture != null) {
                Quad.GetComponent<Renderer>().material.mainTexture = currentTexture;
                this.gameObject.SetActive(true);

                this.transform.localScale
                    = new Vector3(
                        currentTexture.width / data.mapDimensions.x,
                        currentTexture.height / data.mapDimensions.y,
                        1.0f);
            } else {
                this.gameObject.SetActive(false);
            }

            textureChanged = false;
        }
    }

    public void Initialise(BackgroundImageData data) {
        this.data = data;

        this.currentTexture = data.Textures[0];
        this.textureChanged = true;
        this.transform.position = data.Positions[0];
    }
}
