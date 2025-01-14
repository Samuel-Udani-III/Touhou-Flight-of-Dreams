using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;  // Make sure to include this namespace for TMP

public class ScoreManager : MonoBehaviour
{
    // Reference to the TMP_Text component
    public TMP_Text scoreText;  // This will allow you to drag the TMP Text object in the Inspector

    private int score = 0;

    // Time interval for updating the score
    public float updateInterval = 1.0f; // Update the score every 1 second

    // Start is called before the first frame update
    void Start()
    {
        // Start the Score increment coroutine
        StartCoroutine(IncreaseScoreOverTime());
    }

    // Coroutine to increase score every second
    private IEnumerator IncreaseScoreOverTime()
    {
        while (true)
        {
            IncreaseScore(1);  // Increase score by 1
            yield return new WaitForSeconds(updateInterval);  // Wait for the specified time interval
        }
    }

    // Call this method to increase the score and update the display
    public void IncreaseScore(int amount)
    {
        score += amount;
        UpdateScore();
    }

    // Method to update the TMP text to show the current score
    void UpdateScore()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}