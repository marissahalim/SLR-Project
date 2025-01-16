using UnityEngine;
using UnityEngine.UI;

public class RippleController : MonoBehaviour
{
    public Material rippleMaterial;
    public Slider timeSlider; // Slider to select different time points
    public Texture2D[] dataTextures; // Array of textures for different time points
    public float threshold = 0.1f; // Threshold below which ripples start
    public float rippleStrength = 0.1f;
    public float rippleFrequency = 10.0f;

    private void Start()
    {
        // Subscribe to slider value changed event
        timeSlider.onValueChanged.AddListener(HandleSliderChange);
    }

    private void HandleSliderChange(float value)
    {
        int index = Mathf.FloorToInt(value);
        Texture2D currentTexture = dataTextures[index];
        UpdateRippleOrigin(currentTexture);
    }

    void UpdateRippleOrigin(Texture2D texture)
    {
        Vector2 rippleOrigin = FindDarkPoint(texture);
        rippleMaterial.SetFloat("_RippleMagnitude", rippleStrength);
        rippleMaterial.SetFloat("_RippleFrequency", rippleFrequency);
        rippleMaterial.SetFloat("_CustomTime", Time.time);
        rippleMaterial.SetVector("_RippleOrigin", new Vector4(rippleOrigin.x, rippleOrigin.y, 0, 0));
    }

    Vector2 FindDarkPoint(Texture2D texture)
    {
        // Scan the texture to find a dark point (simplified for illustration)
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                Color pixelColor = texture.GetPixel(x, y);
                float grayscale = pixelColor.r * 0.299f + pixelColor.g * 0.587f + pixelColor.b * 0.114f;
                if (grayscale < threshold)
                {
                    // Normalize the coordinates to [0, 1]
                    return new Vector2((float)x / texture.width, (float)y / texture.height);
                }
            }
        }
        return new Vector2(0.5f, 0.5f); // Default to center if no dark point is found
    }
}
