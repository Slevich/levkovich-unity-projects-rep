using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelEnding : MonoBehaviour
{
    [Header("GameObject with screen which show to player when he pass the level")]
    [SerializeField] private GameObject levelEndingScreen;

    [Header("Health Bar of the player")]
    [SerializeField] private GameObject playerHealthBar;

    [Header("Text field with score for time")]
    [SerializeField] private Text timeScore;

    [Header("Text field with score for picking up bonuses")]
    [SerializeField] private Text bonusScore;

    [Header("Text field with score for picking up extra bonuses")]
    [SerializeField] private Text extraBonusScore;

    [Header("Text field with score for killing enemies")]
    [SerializeField] private Text enemiesScore;

    [Header("Text field with total score of the level")]
    [SerializeField] private Text totalScore;

    [Header("Player's GameObject")]
    [SerializeField] private GameObject player;

    private void OnTriggerEnter2D(Collider2D playerCollision)
    {
        if (playerCollision.CompareTag("Player"))
        {
            ActiveLevelEndingScreen();
            CountLevelScore();
        }
    }

    private void ActiveLevelEndingScreen()
    {
        Time.timeScale = 0;
        playerHealthBar.SetActive(false);
        levelEndingScreen.SetActive(true);
    }

    private void CountLevelScore()
    {
        timeScore.text = Mathf.Round(player.GetComponent<ScoreCounter>().timeScore).ToString();
        bonusScore.text = player.GetComponent<ScoreCounter>().bonusScore.ToString();
        extraBonusScore.text = player.GetComponent<ScoreCounter>().extraBonusScore.ToString();
        enemiesScore.text = player.GetComponent<ScoreCounter>().enemiesScore.ToString();
        totalScore.text = player.GetComponent<ScoreCounter>().currentScore.ToString();
    }
}
