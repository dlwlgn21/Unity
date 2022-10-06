using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[System.Serializable]
public class Sound 
{
    public string name;
    public AudioClip clip;

    [Range(0.0f, 1.0f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1.0f;

    [Range(0.0f, 0.5f)]
    public float randomVolume = 0.1f;
    [Range(0.0f, 0.5f)]
    public float randomPitch = 0.1f;

    private AudioSource mAudioSource;

    public bool isLoop;
    public void SetAudioSource(AudioSource audioSource)
    {
        mAudioSource = audioSource;
        mAudioSource.clip = clip;
        mAudioSource.loop = isLoop;
    }

    public void Play()
    {
        mAudioSource.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        mAudioSource.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
        mAudioSource.Play();
    }

    public void Stop()
    {
        mAudioSource.Stop();
    }
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    private Sound[] sounds;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

    }
    void Start()
    {
        for (int i = 0; i < sounds.Length; ++i)
        {
            GameObject gameObejct = new GameObject($"Sound_{i}_{sounds[i].name}");
            gameObejct.transform.parent = transform;
            sounds[i].SetAudioSource(gameObejct.AddComponent<AudioSource>());
        }
        PlaySound("Music");
    }

    public void PlaySound(string name)
    {
        // TODO : 나중에 map으로 바꾸자. 이거 O(N)으로 도는거 맘에 안듦 
        for (int i = 0; i < sounds.Length; ++i)
        {
            if (sounds[i].name == name)
            {
                sounds[i].Play();
                return;
            }
        }

        // no sound with name
        Debug.LogWarning($"AudioManager Can't Found {name}");

    }
    public void StopSound(string name)
    {
        // TODO : 나중에 map으로 바꾸자. 이거 O(N)으로 도는거 맘에 안듦 
        for (int i = 0; i < sounds.Length; ++i)
        {
            if (sounds[i].name == name)
            {
                sounds[i].Stop();
                return;
            }
        }

        // no sound with name
        Debug.LogWarning($"AudioManager Can't Found {name}");

    }
}
