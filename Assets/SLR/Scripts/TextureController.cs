using UnityEngine;
using UnityEngine.UI;

public class TextureController : MonoBehaviour {
    public RawImage rawImage;
    public Slider slider;
    public Texture2D[] images; // Assign these in the editor
    public Texture2D gradientTexture; // Your gradient texture

    void Start() {
        // Create the Texture2DArray from the array of Texture2D images
        Texture2DArray textureArray = CreateTextureArray(images);
        rawImage.material = new Material(Shader.Find("Custom/UnlitTexture2DArrayWithGradientColor"));
        rawImage.material.SetTexture("_MainTex", textureArray);
        rawImage.material.SetTexture("_GradientTex", gradientTexture);

        slider.minValue = 0;
        slider.maxValue = images.Length - 1;
        slider.onValueChanged.AddListener(UpdateTextureSlice);
    }

    void UpdateTextureSlice(float value) {
        rawImage.material.SetInt("_Slice", (int)value);
    }

    // Helper method to create a Texture2DArray from an array of Texture2D
    Texture2DArray CreateTextureArray(Texture2D[] textures) {
        if (textures.Length == 0) return null;

        var texArray = new Texture2DArray(textures[0].width, textures[0].height, textures.Length, textures[0].format, textures[0].mipmapCount > 1);
        texArray.filterMode = FilterMode.Bilinear;
        texArray.wrapMode = TextureWrapMode.Clamp;

        for (int i = 0; i < textures.Length; i++) {
            for (int m = 0; m < textures[0].mipmapCount; m++) {
                Graphics.CopyTexture(textures[i], 0, m, texArray, i, m);
            }
        }

        return texArray;
    }
}
