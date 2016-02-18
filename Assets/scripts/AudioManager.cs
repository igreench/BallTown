using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    public AudioClip MusicMenu;
    public AudioClip MusicGame;

    public AudioClip SoundCombo1;
    public AudioClip SoundCombo2;
    public AudioClip SoundCombo3;
    public AudioClip SoundCombo4;
    public AudioClip SoundCombo5;
    public AudioClip SoundCombo6;
    public AudioClip SoundCombo7;
    public AudioClip SoundCombo8;
    public AudioClip SoundCombo9;
    public AudioClip SoundCombo10;

    public AudioClip SoundExplosion1;
    public AudioClip SoundExplosion2;

    [HideInInspector]
    private float volumeSound = 1f;

    private AudioSource audioSource;

    public static AudioManager instance = null;

	void Awake () {
        audioSource = GetComponent<AudioSource>();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
	}

    public void SetVolumeMusic(float value)
    {
        audioSource.volume = value;
    }

    public void SetVolumeSound(float value)
    {
        volumeSound = value;
    }

    public void PlayMenu()
    {
        audioSource.loop = true;
        audioSource.clip = MusicMenu;
        audioSource.Play();
    }

    public void PlayGame()
    {
        audioSource.loop = true;
        audioSource.clip = MusicGame;
        audioSource.Play();
    }

    public void PlayExplosion()
    {
        audioSource.PlayOneShot(SoundExplosion1, volumeSound);
    }

    public void PlayCombo(int value)
    {
        switch (value)
        {
            case 1:
                audioSource.PlayOneShot(SoundCombo1, volumeSound);
                break;
            case 2:
                audioSource.PlayOneShot(SoundCombo2, volumeSound);
                break;
            case 3:
                audioSource.PlayOneShot(SoundCombo3, volumeSound);
                break;
            case 4:
                audioSource.PlayOneShot(SoundCombo4, volumeSound);
                break;
            case 5:
                audioSource.PlayOneShot(SoundCombo5, volumeSound);
                break;
            case 6:
                audioSource.PlayOneShot(SoundCombo6, volumeSound);
                break;
            case 7:
                audioSource.PlayOneShot(SoundCombo7, volumeSound);
                break;
            case 8:
                audioSource.PlayOneShot(SoundCombo8, volumeSound);
                break;
            case 9:
                audioSource.PlayOneShot(SoundCombo9, volumeSound);
                break;
            case 10:
                audioSource.PlayOneShot(SoundCombo10, volumeSound);
                break;
        }
        
    }

    public void Pause()
    {
        audioSource.Pause();
    }

    public void Play()
    {
        audioSource.Play();
    }
}
