﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyWavesController : MonoBehaviour
{
    #region Переменные
    [Header("Timeline, which plays, when the last enemies wave is dead.")]
    [SerializeField] private PlayableDirector endTimeline;
    [Header("Arrays of enemies in each wave.")]
    [SerializeField] private GameObject[] firstWaveEnemies;
    [SerializeField] private GameObject[] secondWaveEnemies;
    [SerializeField] private GameObject[] thirdWaveEnemies;
    [SerializeField] private GameObject[] fourthWaveEnemies;
    [SerializeField] private GameObject[] enemyWavesObjects;
    [Header("Timer, every iteration of which starting check of alive enemies in wave.")]
    [SerializeField] private float checkTimer;

    //Массив, хранящий врагов текущей волны.
    private GameObject[] currentWave;
    //Номер волны.
    private int waveNumber;
    //Количество живых врагов в волне.
    private int aliveEnemiesInWave;
    //Переменная для обнуления таймера.
    private float currentCheckTimer;
    #endregion

    #region Методы
    /// <summary>
    /// На старте, номер волны равен 1.
    /// Текущая волна равна врагам первой волны.
    /// В переменную с живыми врагами, присваиваем
    /// длину массива текущей волны.
    /// Присваиваем в таймер для обнуления таймер проверки.
    /// </summary>
    private void Start()
    {
        waveNumber = 1;
        currentWave = firstWaveEnemies;
        aliveEnemiesInWave = currentWave.Length;
        currentCheckTimer = checkTimer; 
    }

    /// <summary>
    /// В Update присваиваем в переменную текущей волны,
    /// необходимую волну, в зависимости от номера волны.
    /// Проверяем живых врагов в волне.
    /// Активируем следующую.
    /// </summary>
    private void Update()
    {
        UpdateCurrentWave();
        CheckCurrentWavePerTime();
        ActivateNext();
    }

    /// <summary>
    /// Метод присваивает в текущую волну, в зависимости
    /// от номера волны, ту или иную волну врагов.
    /// </summary>
    private void UpdateCurrentWave()
    {
        switch (waveNumber)
        {
            case 1:
                currentWave = firstWaveEnemies;
                break;

            case 2:
                currentWave = secondWaveEnemies;
                break;

            case 3:
                currentWave = thirdWaveEnemies;
                break;

            case 4:
                currentWave = fourthWaveEnemies;
                break;
        }
    }

    /// <summary>
    /// Метод каждые несколько секунд, проверяет
    /// жив ли враг в волне. Если он равен нулю,
    /// то вычитается единица из количества живых врагов в волне.
    /// </summary>
    private void CheckCurrentWavePerTime()
    {
        checkTimer -= Time.deltaTime;

        if (checkTimer <= 0)
        {
            for (int i = 0; i < currentWave.Length; i++)
            {
                if (currentWave[i] == null)
                {
                    aliveEnemiesInWave--;
                }
            }

            if (aliveEnemiesInWave > 0)
            {
                aliveEnemiesInWave = currentWave.Length;
            }
            checkTimer = currentCheckTimer;
        }
    }

    /// <summary>
    /// Если количество живых врагов в волне становится равно или меньше нуля
    /// и при этом номер волны не больше последней, то активируем следующую волну. 
    /// Увеличиваем номер волны на один. Присваиваем в переменную с количеством
    /// живых врагов - длину массива текущей волны. 
    /// Если номер волны больше последнего, то проигрывается финальная катсцена.
    /// </summary>
    private void ActivateNext()
    {
        if (aliveEnemiesInWave <= 0 && waveNumber <= 4)
        {
            waveNumber++;
            enemyWavesObjects[waveNumber - 1].SetActive(true);
            aliveEnemiesInWave = currentWave.Length;
        }
        else if (waveNumber > 4)
        {
            endTimeline.Play();
        }
    }
    #endregion
}
