/*  Author's Name:          Marcus Ngooi
 *  Last Modified By:       Alexander Maynard
 *  Date Last Modified:     November 25, 2023
 *  Program Description:    Manages sound --> Plays sounds and changes volume.
 *  Revision History:       October 23, 2023: Initial SoundManager script.
 *                          October 24, 2023: Added documentation, adjusted PlayerPref usage.
 *                          November 25, 2023: Added cases for the player sounds in the PlaySfx method switch.
 */

using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// A global manager for game sounds
/// </summary>
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource sfxAudioSource;

    [Header("SFX Audio Clips")]
    [SerializeField] AudioClip buttonClick;
    [SerializeField] AudioClip playerDamage;
    [SerializeField] AudioClip playerDeath;
    [SerializeField] AudioClip phase;
    [SerializeField] AudioClip teleport;
    [SerializeField] AudioClip footstep;
    [SerializeField] AudioClip gunShot;
    [SerializeField] AudioClip jumpLanding;

    [Header("Debug")]
    [SerializeField] private float musicVolume = 1f;
    [SerializeField] private float sfxVolume = 1f;
    [SerializeField] private string musicParameter = "MusicVol";
    [SerializeField] private string sfxParameter = "SfxVol";

    private const float setVolumeMultiplier = 20f;

    public float MusicVolume { get { return musicVolume; } }
    public float SfxVolume { get { return sfxVolume; } }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey(musicParameter))
        {
            SetVolume(PlayerPrefs.GetFloat(musicParameter), SoundType.MUSIC);
        }
        else
        {
            PlayerPrefs.SetFloat(musicParameter, 0.5f);
            SetVolume(PlayerPrefs.GetFloat(musicParameter), SoundType.MUSIC);
        }
        if (PlayerPrefs.HasKey(sfxParameter))
        {
            SetVolume(PlayerPrefs.GetFloat(sfxParameter), SoundType.SFX);
        }
        else
        {
            PlayerPrefs.SetFloat(sfxParameter, 0.5f);
            SetVolume(PlayerPrefs.GetFloat(sfxParameter), SoundType.SFX);
        }
    }
    /// <summary>
    /// A function to change the background music.
    /// </summary>
    /// <param name="clip">The audio clip to be the new background music to play</param>
    public void ChangeMusic(AudioClip clip)
    {
        if (!musicAudioSource.clip.name.Equals(clip.name)) musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }
    /// <summary>
    /// A function to play a sound effect.
    /// </summary>
    /// <param name="sfxEvent">The audio event to trigger the appropriate sound effect</param>
    public void PlaySfx(SfxEvent sfxEvent)
    {
        switch (sfxEvent)
        {
            case SfxEvent.ButtonClick:
                sfxAudioSource.PlayOneShot(buttonClick);
                break;
            case SfxEvent.PlayerDamage:
                sfxAudioSource.PlayOneShot(playerDamage);
                break;
            case SfxEvent.PlayerDeath:
                sfxAudioSource?.PlayOneShot(playerDeath);
                break;
            case SfxEvent.Phase:
                sfxAudioSource?.PlayOneShot(phase);
                break;
            case SfxEvent.Teleport:
                sfxAudioSource.PlayOneShot(teleport);
                break;
            case SfxEvent.GunShot:
                sfxAudioSource.PlayOneShot(gunShot);
                break;
            case SfxEvent.JumpLanding:
                sfxAudioSource.PlayOneShot(jumpLanding);
                break;
        }

    }
    /// <summary>
    /// A function to change the volume.
    /// </summary>
    /// <param name="value">How much to change the volume by.</param>
    /// <param name="type">The type of sound</param>
    public void SetVolume(float value, SoundType type)
    {
        float newValue = Mathf.Log10(value) * setVolumeMultiplier;

        if (value == 0)
        {
            newValue = -100;
        }

        switch (type)
        {
            case SoundType.MUSIC:
                musicVolume = value;
                audioMixer.SetFloat(musicParameter, newValue);
                PlayerPrefs.SetFloat(musicParameter, value);
                break;
            case SoundType.SFX:
                sfxVolume = value;
                audioMixer.SetFloat(sfxParameter, newValue);
                PlayerPrefs.SetFloat(sfxParameter, value);
                break;
            default:
                Debug.LogError("Please assign the sound type before setting volume");
                break;
        }
    }
}
