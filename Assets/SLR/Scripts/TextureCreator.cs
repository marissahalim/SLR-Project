using UnityEngine;
using UnityEngine.UI;  // Required for working with UI

public class TextureCreator : MonoBehaviour
{
    public ComputeShader computeShader;
    public RawImage rawImage;  // Use RawImage to display the texture

    void Start()
    {
        int kernelHandle = computeShader.FindKernel("CSMain");

        RenderTexture texture = new RenderTexture(256, 256, 0);  // Adjust depth to 0 for UI usage
        texture.enableRandomWrite = true;
        texture.Create();

        computeShader.SetTexture(kernelHandle, "Result", texture);
        computeShader.Dispatch(kernelHandle, 32, 32, 1);

        rawImage.texture = texture;  // Set the texture to the RawImage component
    }
}
