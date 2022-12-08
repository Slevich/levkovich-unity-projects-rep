using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseWin : MonoBehaviour
{
    [Header("Minutes in level timer")]
    [SerializeField] private int timerMinutes;

    [Header("Seconds in level timer")]
    [SerializeField] private float timerSeconds;

    [Header("Text field on the screen, which contains minutes of level timer")]
    [SerializeField] private Text timerMinutesText;

    [Header("Text field on the screen, which contains seconds of level timer")]
    [SerializeField] private Text timerSecondsText;

    [Header("Intarface on the level")]
    [SerializeField] private GameObject levelHUD;

    [Header("Screen, which shows when player loses")]
    [SerializeField] private GameObject loseScreen;

    [Header("Audio source, which playing music at the level")]
    [SerializeField] private AudioSource levelMusic;

    //Минуты в таймере на уровне для обнуления.
    private int currentTimerMinutes;
    //Секунды в таймере на уровне для обнуления.
    private float currentTimerSeconds;
    //Булевая переменная для отслеживания - активен ли таймерю
    private bool timerIsActive;
    //Переменная для приведения числа секунд к значению.
    private float secondsInMinute = 59f;

    //На старте обнуляем таймеры. Также значения секунд и минут передаются к текстовые поля, а также таймер становится активен.
    private void Start()
    {
        currentTimerMinutes = timerMinutes;
        currentTimerSeconds = timerSeconds;
        timerSecondsText.text = Mathf.Round(timerSeconds).ToString();
        timerMinutesText.text = timerMinutes.ToString();
        timerIsActive = true;
    }

    //Каждый фрейм передаем в текстовые поля значения секунд и минут. Когда таймер кончается - игрок проигрывает.
    private void Update()
    {
        timerSecondsText.text = Mathf.Round(timerSeconds).ToString();
        timerMinutesText.text = timerMinutes.ToString();

        if (timerIsActive)
        {
            timerSeconds -= Time.deltaTime;

            if (timerSeconds < 0)
            {
                if (timerMinutes > 0)
                {
                    timerSeconds = secondsInMinute;
                    timerMinutes -= 1;
                }
                else if (timerMinutes == 0)
                {
                    timerSeconds = 0;
                    YouLose();
                    timerIsActive = false;
                }
            }
        }
    }

    public void YouLose()
    {
        levelHUD.SetActive(false);
        Time.timeScale = 0; 
        levelMusic.Stop();
        loseScreen.SetActive(true);
    }
}
