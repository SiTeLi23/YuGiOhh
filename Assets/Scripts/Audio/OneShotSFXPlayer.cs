using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotSFXPlayer : MonoBehaviour
{
    [field: SerializeField] public EventReference selectedSFX { get; private set; }

    public void PlaySFX() 
    {
        RuntimeManager.PlayOneShot(selectedSFX, transform.position);
    }
}
