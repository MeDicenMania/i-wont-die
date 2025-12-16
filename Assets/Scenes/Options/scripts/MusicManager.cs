using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource[] sfxSources;

    [Header("Playlists")]
    public AudioClip[] menuPlaylist;
    public AudioClip[] levelPlaylist;
    public bool randomOrder = false;

    private AudioClip[] currentPlaylist;
    private int index = -1;

    private enum PlaylistType { None, Menu, Level }
    private PlaylistType currentPlaylistType = PlaylistType.None;

    public float sfxVolume = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (musicSource == null)
                musicSource = GetComponent<AudioSource>();
            if (musicSource != null)
                musicSource.loop = false;


            float saved = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
            SetMusicVolume(saved);
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
            SetSFXVolume(sfxVolume);

        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (musicSource != null && !musicSource.isPlaying)
            PlayNext();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlaylistType targetType;

        if (scene.name.StartsWith("Level"))
            targetType = PlaylistType.Level;
        else if (scene.name == "Main Menu")
            targetType = PlaylistType.Menu;
        else
            targetType = currentPlaylistType;

        if (targetType != currentPlaylistType)
        {
            currentPlaylistType = targetType;

            if (currentPlaylistType == PlaylistType.Level)
                SetPlaylist(levelPlaylist);
            else if (currentPlaylistType == PlaylistType.Menu)
                SetPlaylist(menuPlaylist);
        }
    }


    public void SetPlaylist(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return;

        currentPlaylist = clips;
        index = -1;
        PlayNext();
    }

    private void PlayNext()
    {
        if (currentPlaylist == null || currentPlaylist.Length == 0) return;

        if (randomOrder)
            index = Random.Range(0, currentPlaylist.Length);
        else
        {
            index++;
            if (index >= currentPlaylist.Length) index = 0;
        }

        musicSource.clip = currentPlaylist[index];
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSources == null || sfxSources.Length == 0) return;

        AudioSource source = sfxSources[0];

        source.PlayOneShot(clip, sfxVolume);
    }

    #region Volume Control

    public void SetMasterVolume(float value)
    {
        value = Mathf.Clamp01(value);
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void SetMusicVolume(float value)
    {
        value = Mathf.Clamp01(value);
        if (musicSource != null)
            musicSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        value = Mathf.Clamp01(value);
        sfxVolume = value;
        if (sfxSources != null)
        {
            foreach (var sfx in sfxSources)
            {
                if (sfx != null) sfx.volume = value;
            }
        }
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    #endregion
}
