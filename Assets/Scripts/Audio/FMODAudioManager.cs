using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMODAudioManager : MonoBehaviour
{
    private List<EventInstance> eventInstances;

    public static FMODAudioManager instance;

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
    }


    public void PlayOneShot(EventReference sound, Vector3 worldPos) 
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
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
