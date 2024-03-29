using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private enum VolumeType 
    {
        MASTER,
        MUSIC,
        SFX
    }

    [Header("Type")]
    [SerializeField] private VolumeType volumeType;

    private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = GetComponentInChildren<Slider>();
    }

    private void Update()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                volumeSlider.value = FMODAudioManager.instance.masterVolume;
                break;
            case VolumeType.MUSIC:
                volumeSlider.value = FMODAudioManager.instance.musicVolume;
                break;
            case VolumeType.SFX:
                volumeSlider.value = FMODAudioManager.instance.sfxVolume;
                break;
            default:
                Debug.LogWarning("Volume Type not supported " + volumeType);
                break;
        }
    }

    public void OnSliderValueChanged() 
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                FMODAudioManager.instance.masterVolume = volumeSlider.value;
                break;
            case VolumeType.MUSIC:
                FMODAudioManager.instance.musicVolume = volumeSlider.value;
                break;
            case VolumeType.SFX:
                FMODAudioManager.instance.sfxVolume = volumeSlider.value;
                break;
            default:
                Debug.LogWarning("Volume Type not supported " + volumeType);
                break;
        }
    }
}
