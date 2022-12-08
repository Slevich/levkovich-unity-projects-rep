using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHUD : MonoBehaviour
{
    [SerializeField] private GameObject pauseFilter;
    [SerializeField] private GameObject BackToLevelMenuButton;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject musicOffButton;
    [SerializeField] private GameObject musicOnButton;
    [SerializeField] private AudioSource levelMusic;
    [SerializeField] private GameObject restartButton;

    private bool musicStopped;

    public void onPressBackToLevelMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void onPressPause()
    {
        Time.timeScale = 0;
        BackToLevelMenuButton.SetActive(false);
        pauseButton.SetActive(false);
        pauseFilter.SetActive(true);
        restartButton.SetActive(false);
        levelMusic.Pause();

        if (musicStopped)
        {
            musicOnButton.SetActive(false);
        }
        else
        {
            musicOffButton.SetActive(false);
        }
    }

    public void onPressBackToGame()
    {
        Time.timeScale = 1;
        pauseFilter.SetActive(false);
        BackToLevelMenuButton.SetActive(true);
        pauseButton.SetActive(true);
        restartButton.SetActive(true);
        
        if (musicStopped)
        {
            musicOnButton.SetActive(true);
        }
        else
        {
            levelMusic.Play();
            musicOffButton.SetActive(true);
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void MusicOff()
    {
        musicOffButton.SetActive(false);
        musicOnButton.SetActive(true);
        levelMusic.Pause();
        musicStopped = true;
    }

    public void MusicOn()
    {
        musicOffButton.SetActive(true);
        musicOnButton.SetActive(false);
        levelMusic.Play();
        musicStopped = false;
    }      
}
