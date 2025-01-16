using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public ParticleSystem particles;
    public Renderer dataRenderer;  // Renderer displaying your data-driven images

    void Update()
    {
        // Example: Change emission rate based on a data value
        ParticleSystem.EmissionModule emission = particles.emission;
        float dataIntensity = GetDataIntensity();  // Implement this function to fetch your current data value
        emission.rateOverTime = dataIntensity * 10;  // Scale emission rate based on data

        // Example: Adjust particle color
        ParticleSystem.MainModule main = particles.main;
        main.startColor = new Color(0.1f, 0.2f, 0.8f, 1.0f);  // Adjust color based on data
    }

    float GetDataIntensity()
    {
        // Implement logic to extract a value from your data that represents "intensity"
        // This might involve analyzing current data values or image properties
        return dataRenderer.material.GetFloat("_DataIntensity");  // Assuming you store a data-related value in the material
    }
}
