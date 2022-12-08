using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform2 : MonoBehaviour
{
    [Header("Last position on X-axis")]
    [SerializeField] private float lastXPosition;

    [Header("Start position on X-axis")]
    [SerializeField] private float firstXPosition;

    [Header("String name of direction to switch")]
    [SerializeField] private string platformDirection;

    //SliderJoint2D платформы для отключения или включения мотора.
    private SliderJoint2D platformSlider;

    private void Awake()
    {
        platformSlider = GetComponent<SliderJoint2D>();
    }

    private void Update()
    {

        if (platformDirection == "Up")
        {
            if (transform.position.x >= lastXPosition)
            {
                platformSlider.useMotor = false;
            }
            else if (transform.position.x <= firstXPosition)
            {
                platformSlider.useMotor = true;
            }
        }
        else if (platformDirection == "Down")
        {
            if (transform.position.x >= lastXPosition)
            {
                platformSlider.useMotor = true;
            }
            else if (transform.position.x <= firstXPosition)
            {
                platformSlider.useMotor = false;
            }
        }
    }
}
