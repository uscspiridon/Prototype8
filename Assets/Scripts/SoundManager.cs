using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static AudioClip ShootSound;
    public static AudioClip DieSound;
    public static AudioClip KillSound;

    public static AudioSource audioSrc;

    float musicVolume = 1f;

    // Start is called before the first frame update
    void Start()
    {
        ShootSound = Resources.Load<AudioClip>("ShootSound");
        DieSound = Resources.Load<AudioClip>("DieSound");
        KillSound = Resources.Load<AudioClip>("KillSound");

        audioSrc = GetComponent<AudioSource>();
    }

    private void Update()
    {
        audioSrc.volume = musicVolume;
    }

    public static void PlaySound(string s)
    {
        switch (s)
        {
            case "ShootSound":
                audioSrc.PlayOneShot(ShootSound);
                break;
            case "DieSound":
                audioSrc.PlayOneShot(DieSound);
                break;
            case "KillSound":
                audioSrc.PlayOneShot(KillSound);
                break;
        }
    }

    public void UpdateVolume(float volume)
    {
        musicVolume = volume;
    }
}
