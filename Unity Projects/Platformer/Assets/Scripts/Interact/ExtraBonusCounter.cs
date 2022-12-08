using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraBonusCounter : MonoBehaviour
{
    [Header("AudioSource which play AudioClip with the sound when ExtraBonus picking up by player")]
    [SerializeField] private AudioSource extraBonusSound;

    [Header("Value of player's score increase")]
    [SerializeField] private float extraBonusScoreIncrease;

    //Аниматор бонуса, чтобы проигрывать анимации.
    private Animator extraBonusAnim;

    private void Awake()
    {
        extraBonusAnim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D playerCollision)
    {
        if (playerCollision.CompareTag("Player"))
        {
            playerCollision.GetComponent<ScoreCounter>().AssessExtraBonusScore(extraBonusScoreIncrease);
            extraBonusAnim.SetBool("PickedUp", true);
        }
    }

    private void DestroyExtraBonus()
    {
        Destroy(gameObject);
    }

    private void PlayPickedUpSound()
    {
        extraBonusSound.Play();
    }
}
