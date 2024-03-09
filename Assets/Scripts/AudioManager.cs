using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private void Awake()
    {
        #region singleTon
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        #endregion
    }

    public AudioSource menuMusic;
    public AudioSource battleSelectMusic;
    public AudioSource[] bgms;
    private int currentBGM;
    private bool playingBGM;

    public AudioSource[] sfx;
    public bool useAudioSystem = true;

    void Update()
    {
     /*   if (playingBGM)
        {
            if (bgms[currentBGM].isPlaying == false)
            {
                currentBGM++;
                if (currentBGM >= bgms.Length)
                {
                    currentBGM = 0;
                }

                bgms[currentBGM].Play();
            }
        }
     */
    }

    public void StopMusic() 
    {
        menuMusic.Stop();
        battleSelectMusic.Stop();
        foreach (AudioSource track in bgms)
        {
            track.Stop();
        }

        playingBGM = false;
    }

    public void PlayMenuMusic() 
    {
        if (useAudioSystem == false) return;
        StopMusic();
        menuMusic.Play();
    }

    public void PlayBattleSelectMusic() 
    {
        if (useAudioSystem == false) return;
        if (battleSelectMusic.isPlaying == false)
        {
            StopMusic();
            battleSelectMusic.Play();
        }
    }

    public void PlayBGM() 
    {
        if (useAudioSystem == false) return;

        StopMusic();

        currentBGM = Random.Range(0, bgms.Length);

        bgms[currentBGM].Play();
        playingBGM = true;
    }

    public void PlaySFX(int sfxToPlay) 
    {
        if (useAudioSystem == false) return;
        sfx[sfxToPlay].Stop();
        sfx[sfxToPlay].Play();
    }
}
