/*
 * Author's Name:           Alexander  Maynard
 * Creation Date:           October 27, 2023
 * Last Modified By:        Alexander Maynard
 * Last Modified Date:      December 3, 2023
 * 
 * Program Description:     This script handles the camera movement of the player and rotation of the player 
 *                          object as well (to make the camera and player X rotations the same).
 *                          
 * Revision History:        October 27, 2023:
 *                              -> Transferred this code from CameraController
 *                              -> modified the code added to work in this script and removed the start
 *                              -> Added more comments
 *                              -> Changed to FixedUpdate for more consistent Lerp to adjust player rotation.
 *                              
 *                          December 3, 2023:
 *                              -> Changed public variables to private and updated comments/comments headers
 */

using UnityEngine;


/// <summary>
/// This class updates the player rotation on the y-axis equal to the y rotation of the camera so that WASD movement keys are updated according the the camera rotation.
/// </summary>
public class UpdatePlayerRotation : MonoBehaviour
{
    //Reference to the player camera
    [SerializeField] private GameObject cam;

    /// <summary>
    /// This is FixedUpdate to be more consistent on updating and be updated more frequently (for less stutter) when the camera moves.
    /// Fixed Update sets the rotation of the player on the y axis to the cameras rotation of the y axis so the player controls are update accordingly.
    /// </summary>
    // Update is called once per frame
    void FixedUpdate()
    {
        //New Quanterion that is updated to match the camera's Y and the players current other values as we do not want to update those
        Quaternion playerNewRotationY = new Quaternion(this.transform.rotation.x, cam.transform.rotation.y, this.transform.rotation.z, cam.transform.rotation.w);
       
        //sets the Y rotation of the player to the same as the camera so the the movement controls match. Time.deltaTime * 1000 controls the interpolation speed
        //Lerp is used here instead of slerp as the movement of the player rotation change is only on one line or axis.
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, playerNewRotationY, 1); // 1 can be  [0 - 1]. It sets the Lerp speed (to a degree).
    }
}
