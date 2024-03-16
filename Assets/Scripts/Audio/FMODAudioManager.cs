using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMODAudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0, 1)]
    public float masterVoluem = 1;
    [Range(0, 1)]
    public float musicVoluem = 1;
    [Range(0, 1)]
    public float sfxVoluem = 1;

    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;


    private List<EventInstance> eventInstances;

    public static FMODAudioManager instance;

    private EventInstance musicEventInstance;

    private void Awake()
    {
        #region singleTon
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        #endregion

        eventInstances = new List<EventInstance>();

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }

    private void Start()
    {

    }

    private void Update()
    {
        masterBus.setVolume(masterVoluem);
        musicBus.setVolume(musicVoluem);
        sfxBus.setVolume(sfxVoluem);
    }

    public void InitializeMusic(EventReference musicEventReference) 
    {
        musicEventInstance = CreateEventInstance(musicEventReference);
        musicEventInstance.start();
    }


    public void PlayOneShot(EventReference sound, Vector3 worldPos) 
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public void SetMusicState(MusicState musicState) 
    {
        musicEventInstance.setParameterByName("State", (float)musicState);
    }

    public EventInstance CreateEventInstance(EventReference eventReference) 
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void CleanUp() 
    {
        // stop and release any created instances
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
