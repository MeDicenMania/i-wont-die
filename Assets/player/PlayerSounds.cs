using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private AudioSource source;

    [Header("Audio Clips")]
    public AudioClip jumpSound;
    public AudioClip dashSound;
    public AudioClip hookThrowSound;
    public AudioClip wallSlideLoop;

    [Header("Pasos")]
    public AudioClip[] footstepSounds;
    public float footstepRate = 0.3f;
    private float footstepTimer;
    private bool isWallSliding = false;

    void Start()
    {
        source = GetComponent<AudioSource>();

        if (MusicManager.Instance != null)
        {
            source.volume = 1f;
        }
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip != null && MusicManager.Instance != null)
        {
            source.PlayOneShot(clip, MusicManager.Instance.sfxVolume);
        }
    }

    public void PlayJump() => PlayClip(jumpSound);
    public void PlayDash() => PlayClip(dashSound);
    public void PlayHook() => PlayClip(hookThrowSound);

    public void HandleFootsteps(bool isMovingOnGround)
    {
        if (isMovingOnGround)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0)
            {
                if (footstepSounds.Length > 0)
                {
                    AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
                    PlayClip(clip);
                }
                footstepTimer = footstepRate;
            }
        }
        else
        {
            footstepTimer = 0;
        }
    }

    public void SetWallSlide(bool isSliding)
    {
        if (isSliding)
        {
            if (!isWallSliding)
            {
                source.clip = wallSlideLoop;
                source.loop = true;

                if (MusicManager.Instance != null)
                    source.volume = MusicManager.Instance.sfxVolume;

                source.Play();
                isWallSliding = true;
            }
            else
            {
                if (MusicManager.Instance != null && source.isPlaying)
                    source.volume = MusicManager.Instance.sfxVolume;
            }
        }
        else
        {
            if (isWallSliding)
            {
                source.Stop();
                source.loop = false;
                isWallSliding = false;
                source.volume = 1f;
            }
        }
    }
}