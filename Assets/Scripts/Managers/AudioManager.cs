using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Sound Effects")]
    public AudioClip jumpSound;
    public AudioClip collectSound;
    public AudioClip hitSound;
    public AudioClip checkpointSound;

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
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayJump() => PlaySFX(jumpSound);
    public void PlayCollect() => PlaySFX(collectSound);
    public void PlayHit() => PlaySFX(hitSound);
    public void PlayCheckpoint() => PlaySFX(checkpointSound);
}