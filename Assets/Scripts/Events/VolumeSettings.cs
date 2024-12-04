using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        masterSlider.onValueChanged.AddListener(delegate { SetVolume(masterSlider, "Master"); });
        musicSlider.onValueChanged.AddListener(delegate { SetVolume(musicSlider, "Music"); });
        sfxSlider.onValueChanged.AddListener(delegate { SetVolume(sfxSlider, "SFX"); });
    }

    private void SetVolume(Slider slider, string mixerParameter)
    {
        float volumeFloat;
        int volumeInt = Mathf.RoundToInt(slider.value);

        if (slider.value < 3)
        {
            volumeFloat = -80 + (float)volumeInt * 20;
        }
        else if (slider.value >= 3 && slider.value < 6)
        {
            volumeFloat = -30 + (float)volumeInt * 5;
        }
        else
        {
            volumeFloat = -10 + (float)volumeInt;
        }

        myMixer.SetFloat(mixerParameter, volumeFloat);
    }
}
