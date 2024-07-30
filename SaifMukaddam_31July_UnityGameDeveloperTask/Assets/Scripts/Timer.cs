using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private float timerDuration = 120f;
    private float timeRemaining;
    [SerializeField] private int totalCubes;

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject winText;


    private ObjectCollectible objectCollectible;

    private bool isGameOver;

    void Start()
    {
        objectCollectible = FindObjectOfType<ObjectCollectible>();
        timeRemaining = timerDuration;
        gameOverText.SetActive(false);
        winText.SetActive(false);
        isGameOver = false;
    }

    void Update()
    {
        if (!isGameOver)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                CheckGameOverConditions();
                isGameOver = true;
            }

            UpdateTimerText();

            if (objectCollectible.collectedItems  >= totalCubes)
            {
                WinGame();
                isGameOver = true;
            }
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void CheckGameOverConditions()
    {
        if (objectCollectible.collectedItems < totalCubes)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        gameOverText.SetActive(true);
        Time.timeScale = 0; // Pause the game
    }

    void WinGame()
    {
        winText.SetActive(true);
        Time.timeScale = 0; // Pause the game
    }
}
