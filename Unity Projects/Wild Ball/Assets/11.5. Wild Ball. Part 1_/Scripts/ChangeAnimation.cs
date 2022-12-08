using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAnimation : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ChangeAnim()
    {
        anim.SetFloat("Speed", Random.value);
        anim.SetFloat("Collided", Random.value);
    }
}
