using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerLevelUI : MonoBehaviour
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

   public void onPressAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void onPressNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void onPressPause()
    {
        Time.timeScale = 0;
        playersHUD.SetActive(false);
        pauseScreen.SetActive(true);

        if (musicStopped == false)
        {
            levelMusic.Pause();
        }
    }

    public void onPressReturn()
    {
        Time.timeScale = 1;
        playersHUD.SetActive(true);
        pauseScreen.SetActive(false);

        if (musicStopped == false)
        {
            levelMusic.Play();
        }
    }

    public void onPressOffMusic()
    {
        musicOffButton.SetActive(false);
        musicOnButton.SetActive(true);
        musicStopped = true;
        levelMusic.Pause();
    }

    public void onPressOnMusic()
    {
        musicOffButton.SetActive(true);
        musicOnButton.SetActive(false);
        musicStopped = false;
        levelMusic.Play();
    }

    public void onPressBackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
