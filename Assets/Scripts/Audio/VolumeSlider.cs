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
                volumeSlider.value = FMODAudioManager.instance.masterVoluem;
                break;
            case VolumeType.MUSIC:
                volumeSlider.value = FMODAudioManager.instance.musicVoluem;
                break;
            case VolumeType.SFX:
                volumeSlider.value = FMODAudioManager.instance.sfxVoluem;
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
                FMODAudioManager.instance.masterVoluem = volumeSlider.value;
                break;
            case VolumeType.MUSIC:
                FMODAudioManager.instance.musicVoluem = volumeSlider.value;
                break;
            case VolumeType.SFX:
                FMODAudioManager.instance.sfxVoluem = volumeSlider.value;
                break;
            default:
                Debug.LogWarning("Volume Type not supported " + volumeType);
                break;
        }
    }
}
