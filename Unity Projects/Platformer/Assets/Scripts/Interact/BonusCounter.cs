using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusCounter : MonoBehaviour
{
    [Header("AudioSource which play AudioClip with the sound when bonus picking up by player")]
    [SerializeField] private AudioSource bonusSound;

    [Header("Value of player's score increase")]
    [SerializeField] private float bonusScoreIncrease;

    //Аниматор бонуса, чтобы проигрывать анимации.
    private Animator bonusAnim;

    private void Awake()
    {
        bonusAnim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D playerCollision)
    {
        if (playerCollision.CompareTag("Player"))
        {
            playerCollision.GetComponent<ScoreCounter>().AssessBonusScore(bonusScoreIncrease);
            bonusAnim.SetBool("IsPickedUp", true);
        }
    }

    private void DestroyCoin()
    {
        Destroy(gameObject);
    }

    private void PlayPickedUpCoinSound()
    {
        bonusSound.Play();
    }
}
