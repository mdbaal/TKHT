using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{ 
    [SerializeField]
    private List<AudioClip> audioClips;

    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioSource effectSource;
    [SerializeField]
    [Range(0.1f, 1f)]
    private float fadeEffectSpeed = .1f;
    [SerializeField]
    private float fadeEffectWaitTime = 1;
    [SerializeField]
    private GameState gameState;


    public void playPickupDropSound()
    {
        if (effectSource.clip == null) effectSource.clip = audioClips[4];
        effectSource.Play();
    }

    public void changeSong()
    {
        if (gameState.inCombat)
        {
            StartCoroutine(transitionAndChangeMusic(Random.Range(2, 4),1));
        }
        else
        {
            StartCoroutine(transitionAndChangeMusic(Random.Range(0, 2),fadeEffectSpeed));
        }
        
    }

    IEnumerator transitionAndChangeMusic(int i,float fadeSpeed)
    {
        while (musicSource.volume > 0 && musicSource.clip != null)
        {
            musicSource.volume -= fadeSpeed;
            yield return new WaitForSecondsRealtime(fadeEffectWaitTime);
        }

        if (i < 4)
        {
            musicSource.clip = audioClips[i];
        }
        else
        {
            musicSource.clip = audioClips[0];
        }

        musicSource.Play();
        
        while (musicSource.volume < 1)
        {
            musicSource.volume += fadeSpeed;
            yield return new WaitForSecondsRealtime(fadeEffectWaitTime);
        }

        
    }
}
