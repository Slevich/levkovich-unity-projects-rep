using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioSource mainMenuMusic;
    [SerializeField] private GameObject firstScreen;
    [SerializeField] private GameObject secondScreen;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject aboutGamePanel;
    [SerializeField] private GameObject aboutDeveloperPanel;

    private GameObject currentScreen;
    private GameObject currentPanel;

    void Start()
    {
        mainMenuMusic.Play();
        firstScreen.SetActive(true);
        currentScreen = firstScreen;
        currentPanel = mainMenuPanel;
    }

    public void ChangeState(GameObject state)
    {
        if (currentScreen != null)
        {
            currentScreen.SetActive(false);
            state.SetActive(true);
            currentScreen = state;
        }
    }

    public void onPressAboutGame()
    {
        currentPanel.SetActive(false);
        aboutGamePanel.SetActive(true);
        currentPanel = aboutGamePanel;
    }

    public void onPressAboutDeveloper()
    {
        currentPanel.SetActive(false);
        aboutDeveloperPanel.SetActive(true);
        currentPanel = aboutDeveloperPanel;
    }

    public void BackToMainMenuPanel()
    {
        currentPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        currentPanel = mainMenuPanel;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
