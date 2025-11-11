using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        if (slider == null) slider = GetComponent<Slider>();

        float saved = PlayerPrefs.GetFloat("musicVolume", 0.8f);
        slider.value = saved;

        slider.onValueChanged.AddListener(OnChanged);
    }

    void OnChanged(float v)
    {
        PlayerPrefs.SetFloat("musicVolume", v);

        if (MusicManager.Instance != null)
            MusicManager.Instance.SetVolume(v);
    }

    void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnChanged);
    }
}
