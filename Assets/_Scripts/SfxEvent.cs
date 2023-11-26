/*  Author's Name:          Marcus Ngooi
 *  Last Modified By:       Alexander Maynard
 *  Date Last Modified:     November 25, 2023
 *  Program Description:    Contains all the Sfx events in the game.
 *  Revision History:       October 24, 2023: Initial SfxEvent enum script.
 *                          November 25, 2023: Added SfxEvent enums for player sounds.
 */

/// <summary>
/// An enum holding all the Sfx events to trigger the appropriate sound effect.
/// </summary>
public enum SfxEvent
{
    ButtonClick,
    PlayerDamage,
    PlayerDeath,
    Phase,
    Teleport,
    JumpLanding,
    GunShot
}
