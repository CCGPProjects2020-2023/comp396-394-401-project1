/*  Author's Name:          Marcus Ngooi
 *  Last Modified By:       Marcus Ngooi
 *  Date Last Modified:     October 24, 2023
 *  Program Description:    Manages the state of the Options Menu. Will notify
 *                          the presenter when its state has changed.
 *  Revision History:       October 24, 2023: Initial Options Menu model script.            
 */

using UnityEngine;

/// <summary>
/// The Model for Options menu.
/// </summary>
public class OptionsMenu : MonoBehaviour
{
    /// <summary>
    /// Changes the volume of the background music.
    /// </summary>
    /// <param name="newVolume">The new volume for the background music</param>
    public void ChangeMusicVolume(float newVolume)
    {
        SoundManager.Instance.SetVolume(newVolume, SoundType.MUSIC);
    }
    /// <summary>
    /// Changes the volume of SFX.
    /// </summary>
    /// <param name="newVolume">The new volume for the SFX</param>
    public void ChangeSfxVolume(float newVolume)
    {
        SoundManager.Instance.SetVolume(newVolume, SoundType.SFX);
    }
}
