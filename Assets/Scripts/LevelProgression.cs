using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelProgression : MonoBehaviour
{
    // Set the number of enemies the player needs to defeat to go to the next level
    public int enemiesToDefeat = 10;

    // Keep track of the current count of defeated enemies
    private int defeatedEnemies = 0;

    // Reference to the player (optional if you want to trigger when the player defeats an enemy)
    public GameObject player;

    // Setting to toggle whether to go to the EndScreen after defeating enough enemies
    public bool goToEndScreen = false;

    void Start()
    {
        // Optionally, check if player reference is assigned
        if (player == null)
        {
            player = GameObject.FindWithTag("Player"); // Assumes your player has the "Player" tag
        }
    }

    // Call this method when an enemy is defeated
    public void EnemyDefeated()
    {
        defeatedEnemies++;

        // Check if enough enemies have been defeated and if we should go to the EndScreen
        if (defeatedEnemies >= enemiesToDefeat)
        {
            if (goToEndScreen)
            {
                GoToEndScreen();
            }
            else
            {
                GoToNextLevel();
            }
        }
    }

    // Method to go to the EndScreen scene (Updated to use "End" instead of "EndScreen")
    void GoToEndScreen()
    {
        // Change the scene name to "End" as per the new scene name
        SceneManager.LoadScene("End");
    }

    // Method to load the next level scene
    void GoToNextLevel()
    {
        // Get the current scene's build index and load the next one
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene exists
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // If there are no more levels, reload the first scene or handle it as needed
            Debug.Log("No more levels. Returning to the first level.");
            SceneManager.LoadScene(0); // Assuming the first scene is index 0
        }
    }
}
