using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEnd : MonoBehaviour
{
    [Header("Interface on the level")]
    [SerializeField] private GameObject levelHUD;

    [Header("Screen, which shows when player succesfully finish the level")]
    [SerializeField] private GameObject winScreen;

    [Header("Audio source, which playing music at the level")]
    [SerializeField] private AudioSource levelMusic;

    [Header("Text field with number of usual bonuses, which player collected at the level")]
    [SerializeField] private Text numberOfUsualBonusesCollected;

    [Header("Text field with number of all usual bonuses, which located at the level")]
    [SerializeField] private Text numberOfUsualBonusesAll;

    [Header("Text field with number of extra bonuses, which player collected at the level")]
    [SerializeField] private Text numberOfExtraBonusesCollected;

    [Header("Text field with number of all extra bonuses, which located at the level")]
    [SerializeField] private Text numberOfExtraBonusesAll;

    //При вхождении в триггер на выходе из уровня, отключаем HUD уровня, останавливаем музыку на уровне, останавливаем время на уровне, активируем экран победы и передаем в текстовые поля количества бонусов.
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            levelHUD.SetActive(false);
            levelMusic.Stop();
            Time.timeScale = 0;
            winScreen.SetActive(true);
            numberOfUsualBonusesCollected.text = collider.GetComponent<PlayerStatistic>().numberOfBonuses.ToString();
            numberOfUsualBonusesAll.text = collider.GetComponent<PlayerStatistic>().allBonusesAtLevel.ToString();
            numberOfExtraBonusesCollected.text = collider.GetComponent<PlayerStatistic>().numberOfExtraBonuses.ToString();
            numberOfExtraBonusesAll.text = collider.GetComponent<PlayerStatistic>().allExtraBonusesAtLevel.ToString();
        }
    }
}
