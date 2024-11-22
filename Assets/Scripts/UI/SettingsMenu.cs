using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider uiSlider;

    private void Start()
    {
        LoadVolumeSettings();
    }

    private void LoadVolumeSettings()
    {
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        float uiVolume = PlayerPrefs.GetFloat("UIVolume", 1.0f);

        audioMixer.SetFloat("MasterVolume", ConvertToLogarithmic(masterVolume));
        audioMixer.SetFloat("MusicVolume", ConvertToLogarithmic(musicVolume));
        audioMixer.SetFloat("SFXVolume", ConvertToLogarithmic(sfxVolume));
        audioMixer.SetFloat("UIVolume", ConvertToLogarithmic(uiVolume));

        if (masterSlider != null) masterSlider.value = masterVolume;
        if (musicSlider != null) musicSlider.value = musicVolume;
        if (sfxSlider != null) sfxSlider.value = sfxVolume;
        if (uiSlider != null) uiSlider.value = uiVolume;
    }

    public void SetMasterVolume(float sliderValue)
    {
        audioMixer.SetFloat("MasterVolume", ConvertToLogarithmic(sliderValue));
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }

    public void SetMusicVolume(float sliderValue)
    {
        audioMixer.SetFloat("MusicVolume", ConvertToLogarithmic(sliderValue));
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);

    }

    public void SetSFXVolume(float sliderValue)
    {
        audioMixer.SetFloat("SFXVolume", ConvertToLogarithmic(sliderValue));
        PlayerPrefs.SetFloat("SFXVolume", sliderValue);
    }

    public void SetUIVolume(float sliderValue)
    {
        audioMixer.SetFloat("UIVolume", ConvertToLogarithmic(sliderValue));
        PlayerPrefs.SetFloat("UIVolume", sliderValue);

    }

    private float ConvertToLogarithmic(float sliderValue)
    {
        return Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1)) * 20;
    }
}
