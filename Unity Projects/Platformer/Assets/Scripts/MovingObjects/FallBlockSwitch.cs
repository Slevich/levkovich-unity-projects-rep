using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallBlockSwitch : MonoBehaviour
{
    //Компонент со скриптом "Falling Block" в родительском объекте, для изменения состояний аниматора.
    private FallingBlock fallingBlock;

    private void Awake()
    {
        fallingBlock = GetComponentInParent<FallingBlock>();
    }

    public void FallingDown()
    {
        fallingBlock.fall = true;
        fallingBlock.blockAnim.SetBool("Go", false);
    }
}
