using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicRiverCrates : MonoBehaviour
{
    [Header("Sraer position on X-axis")]
    [SerializeField] private float startXPosition;

    [Header("Last position on X-axis")]
    [SerializeField] private float finishXPosition;

    private void Update()
    {
        if (transform.position.x > finishXPosition)
        {
            transform.position = new Vector2(startXPosition, transform.position.y);
        }
    }
}
