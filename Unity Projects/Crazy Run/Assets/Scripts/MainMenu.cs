using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panel with buttons in main menu")]
    [SerializeField] private GameObject buttonsPanel;

    [Header("Panel with chosing level in main menu")]
    [SerializeField] private GameObject playPanel;

    [Header("Panel with discription of author in main menu")]
    [SerializeField] private GameObject authorPanel;

    [Header("Panel with discription of game controls in main menu")]
    [SerializeField] private GameObject controlPanel;

    [Header("Button which off music")]
    [SerializeField] private GameObject offMusicButton;

    [Header("Button which on music")]
    [SerializeField] private GameObject onMusicButton;

    [Header("Audio source which playing level music")]
    [SerializeField] private AudioSource mainMenuMusic;

    //Метод активирует панель с выбором уровней.
    public void OnPressPlay()
    {
        buttonsPanel.SetActive(false);
        playPanel.SetActive(true);
    }

    //Метод активирует панель с управлением.
    public void onPressControl()
    {
        buttonsPanel.SetActive(false);
        controlPanel.SetActive(true);
    }

    //Метод активирует панель с информацией об авторе игры.
    public void onPressAuthor()
    {
        buttonsPanel.SetActive(false);
        authorPanel.SetActive(true);
    }

    //Метод возвращает в главный экран по нажатии на кнопку.
    public void onPressBack(GameObject currentPanel)
    {
        currentPanel.SetActive(false);
        buttonsPanel.SetActive(true);
    }

    //Метод запускает соответствующий уровень в меню выбора уровней.
    public void LoadLevel (int i)
    {
        SceneManager.LoadScene(i);
    }

    //Метод выключает музыку в главном меню.
    public void TurnOffMusic()
    {
        offMusicButton.SetActive(false);
        mainMenuMusic.Pause();
        onMusicButton.SetActive(true);
    }

    //Метод, включает музыку в главном меню.
    public void TurnOnMusic()
    {
        onMusicButton.SetActive(false);
        mainMenuMusic.Play();
        offMusicButton.SetActive(true);
    }
}
