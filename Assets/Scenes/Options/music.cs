using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioSource musicSource;
    public AudioClip[] musicClips;
    public bool randomOrder = false;

    int index = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (musicSource == null)
            musicSource = GetComponent<AudioSource>();

        if (musicSource != null)
        {
            musicSource.loop = false;
            PlayNext();
        }

        float saved = PlayerPrefs.GetFloat("musicVolume", 0.8f);
        SetVolume(saved);

    }

    void Update()
    {
        if (musicSource != null && !musicSource.isPlaying)
            PlayNext();
    }

    void PlayNext()
    {
        if (musicClips == null || musicClips.Length == 0) return;

        if (randomOrder)
        {
            index = Random.Range(0, musicClips.Length);
        }
        else
        {
            index++;
            if (index >= musicClips.Length) index = 0;
        }

        musicSource.clip = musicClips[index];
        musicSource.Play();
    }

    public void SetVolume(float value)
    {
        value = Mathf.Clamp01(value);
        if (musicSource != null) musicSource.volume = value;
    }

    public float GetVolume()
    {
        return musicSource != null ? musicSource.volume : 1f;
    }
}
