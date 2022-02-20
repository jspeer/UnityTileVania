using System.Runtime.Serialization;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] float levelLoadDelay = 0.5f;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] int coins = 0;

    void Awake()
    {
        // We only ever want one game session
        int numOfGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numOfGameSessions > 1) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        string livesPlural = playerLives == 1 ? "Life" : "Lives";
        livesText.text = $"{livesPlural}: {playerLives.ToString()}";
        string plurarStr = coins == 1 ? " " : "s";
        coinsText.text = $"{coins.ToString()} Coin" + plurarStr;
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1) {
            playerLives--;
            string livesPlural = playerLives == 1 ? "Life" : "Lives";
            livesText.text = $"{livesPlural}: {playerLives.ToString()}";
            StartCoroutine(TakeLife());
        } else {
            StartCoroutine(ResetGameSession());
        }
    }

    public void AddToScore(int pointsToAdd)
    {
        coins += pointsToAdd;

        string plurarStr = coins == 1 ? " " : "s";
        coinsText.text = $"{coins.ToString()} Coin" + plurarStr;
    }

    IEnumerator ResetGameSession()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        FindObjectOfType<ScenePersistence>().ResetScenePersistence();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    IEnumerator TakeLife()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
