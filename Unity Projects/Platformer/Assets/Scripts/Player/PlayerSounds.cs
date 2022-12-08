using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [Header("Array with footstep sounds")]
    [SerializeField] private AudioClip[] footstepSounds = new AudioClip[15];

    [Header("AudioSource with sounds of player")]
    [SerializeField] private AudioSource playerSoundsSource;

    [Header("AudioClip with sound of sword shoosh")]
    [SerializeField] private AudioClip swordSound;

    [Header("AudioClip with sound of landing after falling")]
    [SerializeField] private AudioClip landingSound;

    [Header("AudioClip with sound of cast spell (shooting)")]
    [SerializeField] private AudioClip castSpellSound;

    //Номер звука из массива который проигрывается.
    private int soundNumber;

    public void PlayFootstepSound()
    {
        soundNumber = Random.Range(0, 14);
        playerSoundsSource.clip = footstepSounds[soundNumber];
        playerSoundsSource.Play();
    }

    public void PlaySwordSwoosh()
    {
        playerSoundsSource.clip = swordSound;
        playerSoundsSource.Play();
    }

    public void PlayLandingSound()
    {
        playerSoundsSource.clip = landingSound;
        playerSoundsSource.Play();
    }

    public void PlayCastSpellSound()
    {
        playerSoundsSource.clip = castSpellSound;
        playerSoundsSource.Play();
    }
}
