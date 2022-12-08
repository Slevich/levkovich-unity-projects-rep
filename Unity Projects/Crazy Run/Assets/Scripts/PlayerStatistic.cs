using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistic : MonoBehaviour
{
    [Header("Number of usual bonuses earned by player at the level")]
    public int numberOfBonuses = 0;

    [Header("Number of all usual bonuses at the level")]
    public int numberOfExtraBonuses = 0;

    [Header("Number of extra bonuses earnded by player at the level")]
    public int allBonusesAtLevel;

    [Header("Number of all extra bonuses at the level")]
    public int allExtraBonusesAtLevel;


    //На старте текущее количество бонусов обнуляется.
    private void Start()
    {
        numberOfBonuses = 0;
        numberOfExtraBonuses = 0;
    }


}
