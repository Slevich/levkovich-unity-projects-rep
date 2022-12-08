using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChooseLevelMenu : MonoBehaviour
{
    [SerializeField] private GameObject chooseLevelScreen;
    [SerializeField] private GameObject mainMenuScreen;

    public void onPressBackToMainMenu()
    {
        chooseLevelScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
    }

    public void onPressChooseLevel(int n)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + n);
    }
}
