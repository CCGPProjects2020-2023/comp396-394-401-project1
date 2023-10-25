/*  Author's Name:          Marcus Ngooi
 *  Last Modified By:       October 23, 2023
 *  Date Last Modified:     October 24, 2023
 *  Program Description:    Manages sound --> Plays sounds and changes volume.
 *  Revision History:       October 23, 2023: Initial SoundManager script.
 *                          October 24, 2023: Added documentation, adjusted PlayerPref usage.
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
