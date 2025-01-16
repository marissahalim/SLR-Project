using UnityEngine;
using UnityEngine.UI; // Needed for UI components

public class CheckGrayscaleRange : MonoBehaviour
{
    public Material material; // Material that uses your shader
    private Texture2D mainTex;

    void Start()
    {
        // Get the Image component from the GameObject
        Image image = GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("No Image component found on this GameObject!");
            return;
        }

        // Get the material assigned to the Image component
        material = image.material;
        if (material == null)
        {
            Debug.LogError("No material assigned to the Image!");
            return;
        }

        // Get the _MainTex from the material
        mainTex = material.GetTexture("_MainTex") as Texture2D;
        if (mainTex == null)
        {
            Debug.LogError("Texture is not a Texture2D or not assigned to _MainTex!");
            return;
        }

        // Check grayscale range
        Color[] pixels = mainTex.GetPixels();
        float minVal = 1.0f, maxVal = 0.0f;

        foreach (Color pixel in pixels)
        {
            float grayscale = pixel.r * 0.299f + pixel.g * 0.587f + pixel.b * 0.114f;

            if (grayscale < minVal) minVal = grayscale;
            if (grayscale > maxVal) maxVal = grayscale;
        }

        Debug.Log($"Grayscale range: Min={minVal}, Max={maxVal}");
    }
}
