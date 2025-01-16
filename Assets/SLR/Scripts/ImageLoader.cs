using UnityEngine;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour
{
    public string[] imagePaths;  // Paths to the images in the Resources folder
    public Material colorizedMaterial; // Add this line at the top with other public variables

    public Image displayImage;   // Reference to the UI Image component
    public Slider imageSlider;   // Reference to the UI Slider component
    private Sprite[] loadedSprites;

    public control controller;
    public int currentIndex = 0;

    void Start()
    {
        // Initialize the sprite array based on the number of image paths
        loadedSprites = new Sprite[imagePaths.Length];
        // imageSlider.maxValue = imagePaths.Length - 1; // Set the slider's maximum value

        currentIndex = (int)imageSlider.value;
        Debug.Log(currentIndex);

        // Load each image as a Texture2D and create a Sprite from it
        for (int i = 0; i < imagePaths.Length; i++)
        {
            Texture2D texture = Resources.Load<Texture2D>(imagePaths[i]);
            if (texture == null)
            {
                Debug.LogError("Failed to load texture at path: " + imagePaths[i]);
                continue;
            }
            loadedSprites[i] = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        }

        // Setup the slider event listener
        imageSlider.onValueChanged.AddListener(delegate
        {
            currentIndex = (int)imageSlider.value;
            Debug.Log(currentIndex);
            UpdateImage(currentIndex);
        });
        UpdateImage(currentIndex);  // Update the image at startup
    }

    //private void Update()
    //{
    //    UpdateImage((int)controller.currentState);
    //}

    public void UpdateImage(int index)
    {
        // index = (int)imageSlider.value;
        if (loadedSprites[index] != null)
        {
            displayImage.material = colorizedMaterial; // Use the custom material
            displayImage.sprite = loadedSprites[index];
        }
    }



}

