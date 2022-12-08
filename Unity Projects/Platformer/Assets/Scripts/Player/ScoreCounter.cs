using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    [Header("Text field with score of player at level")]
    [SerializeField] private Text scoreText;

    [Header("Current score of the player at the level")]
    [SerializeField] public float currentScore;

    [Header("Score of the playing time at the level")]
    [SerializeField] public float timeScore;

    [Header("Score for picking up bonuses")]
    [SerializeField] public float bonusScore;

    [Header("Score for picking up extra bonuses")]
    [SerializeField] public float extraBonusScore;

    [Header("Score for killing enemies")]
    [SerializeField] public float enemiesScore;

    private void Start()
    {
        currentScore = 0;
        timeScore = 0;
        bonusScore = 0;
        extraBonusScore = 0;
        enemiesScore = 0; 
    }

    private void Update()
    {
        AssessTotalScore();
        scoreText.text = currentScore.ToString();
        timeScore += Time.deltaTime;
    }

    public void AssessTotalScore()
    {
        currentScore = bonusScore + extraBonusScore + enemiesScore;
    }
    public void AssessBonusScore(float bonusScoreIncrese)
    {
        bonusScore += bonusScoreIncrese;
    }
    public void AssessExtraBonusScore(float extraBonusScoreIncrese)
    {
        extraBonusScore += extraBonusScoreIncrese;
    }

    public void AssessEnemiesDeathScore(float enemyScoreIncrese)
    {
        enemiesScore += enemyScoreIncrese;
    }
}
