using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelHUD : MonoBehaviour
{
    [Header("AudioSource which plays music in main menu")]
    [SerializeField] private AudioSource levelMusic;

    [Header("HUD on the level")]
    [SerializeField] private GameObject playersHUD;

    [Header("Screen when game on pause")]
    [SerializeField] private GameObject pauseScreen;

    [Header("Button which off music")]
    [SerializeField] private GameObject musicOffButton;

    [Header("Button which on music")]
    [SerializeField] private GameObject musicOnButton;

    //Переменная bool, которая отслеживает остановлена ли музыка.
    private bool musicStopped;
    //Переменная обозначающая, на паузе ли игра.
    public bool gameOnPause;

    //На старте ставим время в обычный режим, а также переменную, обозначающую на паузе ли игра, ставим в true.
    private void Start()
    {
        Time.timeScale = 1;
        gameOnPause = false;
    }

    //Метод, запускающий уровень заново при нажатии на кнопку.
    public void onPressAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Метод, запускающий следующий уровень по нажатии на кнопку. Если это последний уровень, запускается первый.
    public void onPressNext()
    {
        if (SceneManager.GetActiveScene().buildIndex < 4)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            SceneManager.LoadScene(1);
        }
    }

    //Метод, ставящий время на паузу, отключающий HUD на уровне, включается экран паузы, переменная переводится в значение "игра на паузе". Если музыка на уровне была на паузе - это продолжается.
    public void onPressPause()
    {
        Time.timeScale = 0;
        playersHUD.SetActive(false);
        pauseScreen.SetActive(true);
        gameOnPause = true;

        if (musicStopped == false)
        {
            levelMusic.Pause();
        }
    }

    //Метод возвращающий обратно в игру: возвращаем время в обычный режим, показываем HUD уровня, скрываем экран паузы, переменная "игра на паузе" становится false. 
    public void onPressReturn()
    {
        Time.timeScale = 1;
        playersHUD.SetActive(true);
        pauseScreen.SetActive(false);
        gameOnPause = false;

        if (musicStopped == false)
        {
            levelMusic.Play();
        }
    }

    //Метод, ставящий музыку на уровне на паузу и меняющий кнопку выключения на кнопку включения.
    public void onPressOffMusic()
    {
        musicOffButton.SetActive(false);
        musicOnButton.SetActive(true);
        musicStopped = true;
        levelMusic.Pause();
    }

    //Метод, запускающий музыку на уровне и меняющий кнопку вкючения на кнопку выключения.
    public void onPressOnMusic()
    {
        musicOffButton.SetActive(true);
        musicOnButton.SetActive(false);
        musicStopped = false;
        levelMusic.Play();
    }

    //Метод, запускающий сцену главного меню по нажатии на кнопку.
    public void onPressBackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
