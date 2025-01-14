using UnityEngine;
using UnityEngine.SceneManagement;  // Required for scene management
using UnityEngine.UI;  // Required to handle UI button

public class SceneLoader : MonoBehaviour
{
    public Button loadSceneButton;  // Reference to the button

    void Start()
    {
        // Ensure that the button is linked
        if (loadSceneButton != null)
        {
            loadSceneButton.onClick.AddListener(OnButtonClick);  // Add listener for button click
        }
        else
        {
            Debug.LogError("Button is not assigned!");
        }
    }

    void OnButtonClick()
    {
        // Load the next scene, "Level 0"
        SceneManager.LoadScene("Level 0");  // Replace "Level 0" with your actual scene name
    }
}
