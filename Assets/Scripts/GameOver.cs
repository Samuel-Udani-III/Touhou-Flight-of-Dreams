using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverPanel;

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
    }

    public void Restart()
    {
        // Use a coroutine to restart with a slight delay
        StartCoroutine(RestartAfterDelay());
    }

    private IEnumerator RestartAfterDelay()
    {
        Time.timeScale = 1f; // Ensure time is resumed before the scene reload
        yield return null; // Wait until the next frame

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reloads the current scene
    }

    // This will be called when the scene has finished loading
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called when a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f; // Ensure the game is unpaused after the scene loads
    }
}
