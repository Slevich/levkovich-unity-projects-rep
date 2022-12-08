using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class CutsceneController : MonoBehaviour
{
    [Header("HUD on the level")]
    [SerializeField] private GameObject levelHUD;
    [Header("HUD in Cutscene")]
    [SerializeField] private GameObject cutsceneHUD;

    [Header("HUD with Skup-button")]
    [SerializeField] private GameObject skipCanvas;

    [Header("Virtual Camera which shoot Knight close")]
    [SerializeField] private CinemachineVirtualCamera knightCloseCamera;

    [Header("Virtual Camera which shoot Enemy close")]
    [SerializeField] private CinemachineVirtualCamera enemyCloseCamera;

    [Header("Virtual Camera which shoot Enemy very close")]
    [SerializeField] private CinemachineVirtualCamera enemySoCloseCamera;

    [Header("Virtual Camera which shoot overall scene")]
    [SerializeField] private CinemachineVirtualCamera overallCamera;

    [Header("Virtual Camera which uses in the gameplay on the level")]
    [SerializeField] private CinemachineVirtualCamera levelCamera;

    [Header("Сharacters lines")]
    [SerializeField] private Text cutsceneText;

    [Header("Audio Source where plays sounds at cutscene")]
    [SerializeField] private AudioSource cutsceneAudioSource;

    [Header("All cutscene sounds")]
    [SerializeField] private AudioClip[] cutsceneSounds = new AudioClip[4];

    [Header("Object with Knight's appearance animation")]
    [SerializeField] private GameObject appearanceFX;

    [Header("Player's gameobject")]
    [SerializeField] private GameObject player;

    [Header("Enemy gameobject fot cutscene")]
    [SerializeField] private GameObject enemy;

    [Header("Enemy stoppers triggers")]
    [SerializeField] private GameObject stoppers;

    [Header("Player's Rigidbody2D")]
    [SerializeField] private Rigidbody2D playersRB;

    [Header("All timers for which changes cameras")]
    [SerializeField] private float[] cutsceneTimers = new float[7];

    //Переменная типа bool для переключения режима катсцена/уровень.
    private bool cutsceneIsActive;

    //Переменная для активации эффекта появления ГГ, чтобы не было null при его удалении.
    private bool appearanceFXIsActivate;

    //Переменная для определения номера, какой таймер идёт.
    private int timerNumber;


    private void Awake()
    {
        cutsceneIsActive = true;
        levelHUD.SetActive(false);
        timerNumber = 0;
    }

    private void Update()
    {
        if (cutsceneIsActive)
        {
            player.GetComponent<Platformer.Inputs.PlayerInput>().inputIsActive = false;
            cutsceneTimers[timerNumber] -= Time.deltaTime;

            if (cutsceneTimers[timerNumber] < 0)
            {
                if (timerNumber == 0)
                {
                    FirstPlan();
                    timerNumber += 1;
                }
                else if (timerNumber == 1)
                {
                    SecondPlan();
                    timerNumber += 1;
                }
                else if (timerNumber == 2)
                {
                    ThirdPlan();
                    timerNumber += 1;
                }
                else if (timerNumber == 3)
                {
                    FourthPlan();
                    timerNumber += 1;
                }
                else if (timerNumber == 4)
                {
                    FifthPlan();
                    timerNumber += 1;
                }
                else if (timerNumber == 5)
                {
                    SixthPlan();
                    timerNumber += 1;
                }
                else if (timerNumber == 6)
                {
                    SeventhPlan();
                    timerNumber += 1;
                }
                else if (timerNumber == 7)
                {
                    EighthPlan();
                }
            }
        }
    }

    private void FirstPlan()
    {
        appearanceFXIsActivate = true;
        if (appearanceFXIsActivate)
        {
            appearanceFX.SetActive(true);
            appearanceFXIsActivate = false;
        }
        player.SetActive(true);
    }

    private void SecondPlan()
    {
        overallCamera.Priority = 9;
        knightCloseCamera.Priority = 10;
        cutsceneHUD.SetActive(true);
        cutsceneText.text = "Knight: Where Am I?";
        cutsceneAudioSource.clip = cutsceneSounds[0];
        cutsceneAudioSource.Play();
    }

    private void ThirdPlan()
    {
        knightCloseCamera.Priority = 8;
        overallCamera.Priority = 10;
        enemy.GetComponent<EnemyCutsceneController>().goForward = true;
        cutsceneHUD.SetActive(false);
    }

    private void FourthPlan()
    {
        overallCamera.Priority = 9;
        enemyCloseCamera.Priority = 10; 
        cutsceneHUD.SetActive(true);
        cutsceneText.text = "Lowest Demon: You are trapped in one of the forgotten worlds, knight...";
        cutsceneAudioSource.clip = cutsceneSounds[1];
        cutsceneAudioSource.Play();
    }

    private void FifthPlan()
    {
        enemySoCloseCamera.Priority = 11;
        cutsceneText.text = "Lowest Demon: and there is no way out.";
        cutsceneAudioSource.clip = cutsceneSounds[2];
        cutsceneAudioSource.Play();
    }

    private void SixthPlan()
    {
        enemySoCloseCamera.Priority = 8;
        knightCloseCamera.Priority = 11;
        cutsceneText.text = "Knight: We shall see.";
        cutsceneAudioSource.clip = cutsceneSounds[3];
        cutsceneAudioSource.Play();
    }

    private void SeventhPlan()
    {
        knightCloseCamera.Priority = 8;
        overallCamera.Priority = 11;
        enemy.GetComponent<EnemyCutsceneController>().goBackward = true;
        cutsceneHUD.SetActive(false);
    }

    private void EighthPlan()
    {
        levelHUD.SetActive(true);
        cutsceneIsActive = false;
        Destroy(enemy);
        Destroy(stoppers);
        skipCanvas.SetActive(false);
        levelCamera.Priority = 12;
        gameObject.SetActive(false);
        player.GetComponent<Platformer.Inputs.PlayerInput>().inputIsActive = true;
    }

    public void onPressSkip()
    {
        levelHUD.SetActive(true);
        cutsceneHUD.SetActive(false);
        cutsceneIsActive = false;
        Destroy(enemy);
        Destroy(stoppers);
        levelCamera.Priority = 12;
        skipCanvas.SetActive(false);
        player.SetActive(true);
        appearanceFX.SetActive(false);
        gameObject.SetActive(false);
        player.GetComponent<Platformer.Inputs.PlayerInput>().inputIsActive = true;
    }
}
