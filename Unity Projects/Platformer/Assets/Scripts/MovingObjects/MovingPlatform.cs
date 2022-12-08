using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Last position of the  platform")]
    [SerializeField] private Vector3 lastPosition;

    [Header("Timer of platfrorm waiting")]
    [SerializeField] private float waitTimer;

    [Header("Start position on X-axis")]
    [SerializeField] private float startXPosition;

    [Header("Last position on X-axis")]
    [SerializeField] private float endXPosition;

    //Таймер ожидания для обнуления.
    private float currentWaitTimer;

    //SliderJoint2D платформы для отключения или включения мотора.
    private SliderJoint2D platformSJ;

    private void Start()
    {
        currentWaitTimer = waitTimer;
        platformSJ = GetComponent<SliderJoint2D>();
    }

    private void Update()
    {
        if (transform.position.x >= endXPosition)
        {
            platformSJ.useMotor = false;
        }
        else if (transform.position.x <= startXPosition)
        {
            platformSJ.useMotor = true;
        }
    }
}
