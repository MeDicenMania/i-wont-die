using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        if (MusicManager.Instance == null)
        {
            Debug.LogWarning("MusicManager not found! Make sure it's in a persistent scene.");
            return;
        }

        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.8f);

        masterSlider.onValueChanged.AddListener(MusicManager.Instance.SetMasterVolume);
        musicSlider.onValueChanged.AddListener(MusicManager.Instance.SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(MusicManager.Instance.SetSFXVolume);
    }

    void OnDestroy()
    {
        if (masterSlider != null) masterSlider.onValueChanged.RemoveAllListeners();
        if (musicSlider != null) musicSlider.onValueChanged.RemoveAllListeners();
        if (sfxSlider != null) sfxSlider.onValueChanged.RemoveAllListeners();
    }
}