using UnityEngine;
using UnityEngine.UI;

public class DataLayerImages : MonoBehaviour
{
    public Slider textureSliceSlider; // Slider for controlling the texture slice
    public Button scenario1Button;    // Button to activate scenario1
    public Button scenario2Button;    // Button to activate scenario2
    public GameObject scenario1;      // GameObject for the first scenario layer
    public GameObject scenario2;      // GameObject for the second scenario layer

    public Material scenario1Material; // Material for scenario1
    public Material scenario2Material; // Material for scenario2

    private GameObject currentActiveLayer; // Tracks the currently active layer
    private Material currentActiveMaterial; // Stores the current material of the active layer

    void Start()
    {
        // Set default active layer to scenario1
        ActivateLayer(scenario1, scenario1Material);

        // Set up the slider for controlling texture slices
        textureSliceSlider.minValue = 0;
        textureSliceSlider.maxValue = 8; // Assuming 9 slices (0 through 8)
        textureSliceSlider.onValueChanged.AddListener(UpdateTextureSlice);

        // Add button click listeners
        scenario1Button.onClick.AddListener(() => ActivateLayer(scenario1, scenario1Material));
        scenario2Button.onClick.AddListener(() => ActivateLayer(scenario2, scenario2Material));
    }

public void ActivateLayer(GameObject layer, Material material)
{
    if (currentActiveLayer != null)
        currentActiveLayer.SetActive(false);

    currentActiveLayer = layer;
    currentActiveMaterial = material;

    currentActiveLayer.SetActive(true);

    // Try to get the Renderer from the current layer, or from its children if not found
    Renderer renderer = currentActiveLayer.GetComponent<Renderer>();
    if (renderer == null)
    {
        renderer = currentActiveLayer.GetComponentInChildren<Renderer>(true); // true to include inactive children
    }

    if (renderer != null)
    {
        renderer.material = currentActiveMaterial;
    }
    else
    {
        Debug.LogError("Renderer not found on the activated scenario.");
    }

    // Update the texture slice based on the current value of the slider
    UpdateTextureSlice(textureSliceSlider.value);
}


    private void UpdateTextureSlice(float value)
    {
        if (currentActiveMaterial != null)
        {
            int sliceIndex = Mathf.FloorToInt(value);
            currentActiveMaterial.SetInt("_Slice", sliceIndex); // Update the slice index on the material
        }
        else
        {
            Debug.LogError("Current active material is null.");
        }
    }
}
