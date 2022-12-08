using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panel with level's choice")]
    [SerializeField] private GameObject chooseLevelPanel;

    [Header("Panel with information about game")]
    [SerializeField] private GameObject aboutGamePanel;

    [Header("Panel with information about developer")]
    [SerializeField] private GameObject aboutDeveloperPanel;

    [Header("Button which off music")]
    [SerializeField] private GameObject musicOffButton;

    [Header("Button which on music")]
    [SerializeField] private GameObject musicOnButton;

    [Header("AudioSource which plays music in main menu")]
    [SerializeField] private AudioSource mainMenuMusic;

    public void onPressPlay()
    {
        chooseLevelPanel.SetActive(true);
        aboutGamePanel.SetActive(false);
        aboutDeveloperPanel.SetActive(false);
    }

    public void onPressAboutGame()
    {
        chooseLevelPanel.SetActive(false);
        aboutGamePanel.SetActive(true);
        aboutDeveloperPanel.SetActive(false);
    }
    
    public void onPressAboutDeveloper()
    {
        chooseLevelPanel.SetActive(false);
        aboutGamePanel.SetActive(false);
        aboutDeveloperPanel.SetActive(true);
    }

    public void onPressChooseLevel(int n)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + n);
    }

    public void MusicOff()
    {
        musicOffButton.SetActive(false);
        musicOnButton.SetActive (true);
        mainMenuMusic.Pause();
    }

    public void MusicOn()
    {
        musicOffButton.SetActive(true);
        musicOnButton.SetActive(false);
        mainMenuMusic.Play();
    }
}
