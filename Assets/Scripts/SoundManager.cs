using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;
    private AudioSource audioSource;

    // Dictionary to map scene names to their corresponding audio clips
    [SerializeField] private List<SceneMusicMapping> sceneMusicMappings;

    [System.Serializable]
    public struct SceneMusicMapping
    {
        public string sceneName;
        public AudioClip musicClip;
    }

    private string currentMusicScene = ""; // To track the current music scene (Levels 0-3 should share the same music)

    void Awake()
    {
        // Singleton pattern to make sure only one instance of the audio manager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep the audio manager across scenes
            audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate audio managers
        }
    }

    void Start()
    {
        // Play music for the current scene when the game starts
        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    // This method will be used to play the music when a new scene is loaded
    public void PlayMusicForScene(string sceneName)
    {
        // If we're transitioning between Level 0 to Level 3, don't restart the music
        if (sceneName.StartsWith("Level") && currentMusicScene.StartsWith("Level"))
        {
            // If we are already in the same music set (Levels 0-3), don't reset the music
            return;
        }

        // Find the music clip corresponding to the scene
        SceneMusicMapping? sceneMusic = sceneMusicMappings.Find(mapping => mapping.sceneName == sceneName);

        if (sceneMusic.HasValue)
        {
            // If the clip is already playing and it's the same music, do nothing
            if (audioSource.isPlaying && audioSource.clip == sceneMusic.Value.musicClip)
            {
                return;
            }

            // If music is already playing, stop it
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            // Set the new clip and play it
            audioSource.clip = sceneMusic.Value.musicClip;
            audioSource.Play();

            // Update the current music scene to track this music
            currentMusicScene = sceneName;
        }
        else
        {
            Debug.LogWarning("No music found for scene: " + sceneName); // If no music found for the scene
        }
    }

    // Subscribe to the scene-loaded event
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Unsubscribe from the scene-loaded event
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // This is called when a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name); // Play the correct music when the new scene loads
    }
}
