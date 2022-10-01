using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource efxSrc;
    public AudioSource musicSrc;

    public static SoundManager instance = null;

    public float lowPitchRange = 0.95f;
    public float highPitchRange = 1.05f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    public void PlaySingle(AudioClip clip)
    {
        efxSrc.clip = clip;
        efxSrc.Play();
    }

    public void PlayRandomSfx(params AudioClip[] clips)
    {
        int randomIdx = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        efxSrc.pitch = randomPitch;
        efxSrc.clip = clips[randomIdx];
        efxSrc.Play();
    }
}
