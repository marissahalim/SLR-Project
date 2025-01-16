using UnityEngine;
using UnityEngine.UI; // Ensure you have this using directive to handle UI components.

/*
Setting Up:
- Material: Drag the material using your shader into the material field in the script component.
- Slider: Create a UI slider (from the GameObject menu, select UI -> Slider) and assign it to the slider field in the script component.
- Configure Slider: Set the slider's minimum and maximum values based on the available range of slices in the texture array.
*/

public class TextureSliceController : MonoBehaviour
{
    public Material material; // Assign this through the inspector
    public Slider slider;     // Assign the UI slider through the inspector

    void Start()
    {
        // Optionally set slider values based on what you know about the number of slices
        slider.minValue = 0;
        slider.maxValue = 10; // Change this to your maximum slice index
        slider.onValueChanged.AddListener(UpdateTextureSlice);
    }

    void UpdateTextureSlice(float value)
    {
        int sliceIndex = Mathf.FloorToInt(value);
        material.SetInt("_Slice", sliceIndex); // Update the slice index on the material
    }
}
