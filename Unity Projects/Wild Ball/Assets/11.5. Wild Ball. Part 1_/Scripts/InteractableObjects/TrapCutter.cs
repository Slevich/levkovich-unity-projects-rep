using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapCutter : MonoBehaviour
{
    [SerializeField] private float rotatingAngle;

    void Update()
    {
        cutterRotation();
    }

    private void cutterRotation()
    {
        transform.Rotate(0, rotatingAngle, 0);
    }
}
