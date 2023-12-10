/* 
 * Author's Name:           Alexander  Maynard
 * Creation Date:           October 26, 2023
 * Last Modified By:        Alexander Maynard
 * Last Modified Date:      December 10, 2023
 * 
 * Program Description:     This script handles the camera movement of the player and rotation of the player 
 *                          object as well (to make the camera and player X rotations the same).
 *                          
 * Revision History:        October 26, 2023:
 *                              -> Added first variables.
 *                              -> Added list of resources used to help make the camera turn with mouse input.
 *                              -> Added code in the start to lock the cursor and make it invisible.
 *                              -> Added code in the update to move the camera X and Y using the camera speed.
 *                              -> Added links to references used to help learn how to make the camera follow the mouse input.
 *                              -> Added code to pair the player object and camera X rotations together.
 *                              -> Added initial fix for player rotation for movement matching the camera's rotation
 *                              
 *                          October 27, 2023:
 *                              -> Added more fixes to the player rotation matching only the camera on the x axis.
 *                              -> transferred the player rotations adjustments to a new script named UpdatePlayerRotation
 *                              
 *                          November 10, 2023: 
 *                              -> Added player camera separate from the player so added code here for the camera to follow the player 
 *                                  instead of the camera being a parent. This removed some negative behaviours
 *                              ->Added comments to reflect this
 *                              
 *                          November 30, 2023:
 *                              -> Changed values for the camera to fit the soldier Asset purchased from the Unity Store (used for the player)
 *                              
 *                          December 1, 2023:
 *                              -> Changed values for the camera to fit the soldier Asset purchased from the Unity Store (used for the player) again.
 *                              
 *                          December 3, 2023:
 *                              -> Changed public variables to private and updated comments/comments headers
 * 
 *                          December 7, 2023:
 *                              -> Refactored code to include transform/rotation caching for the camera in the hopes to improve efficiency.
 *                              -> General refactoring as well.
 *                          December 10, 2023:
 *                              -> Refactored code to get a local reference of the player position to be more efficient instead of repeated calls
 *                                  to access the playerTransform.position. x, y or z (inefficient).
 */


using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

/// <summary>
/// This class take the mouse input and translates that camera movement to in game. 
/// It also tracks the position of the player and follows the player where specified. 
/// </summary>
public class CameraController : MonoBehaviour
{
    //*** NOTE: Resources used to learn on how to move the camera with mouse input:
    // https://discussions.unity.com/t/setting-main-camera-rotation/43221
    // https://gamedev.stackexchange.com/questions/104693/how-to-use-input-getaxismouse-x-y-to-rotate-the-camera
    // https://forum.unity.com/threads/how-to-lock-or-set-the-cameras-z-rotation-to-zero.68932/#post-441968
    // https://discussions.unity.com/t/how-do-i-move-a-camera-with-mouse/194032/3

    [Header("Player Transform Reference")]
    //Reference to the player object
    [SerializeField] private Transform playerTransform;

    [Header("Camera Values")]
    //cameraX and Y speeds
    [SerializeField] private float cameraSpeedX = 4;
    [SerializeField] private float cameraSpeedY = 4;
    //values for camera X and Y positions
    [SerializeField] private float cameraPitch = 0.0f;
    [SerializeField] private float cameraYaw = 0.0f;
    //values to limit the yaws (how far up and down the mouse can travel)
    [SerializeField] private float yawLimitUpper = -40;
    [SerializeField] private float yawLimitLower = 40;
    
    //transform caching for the camera and player
    private Transform camTransform;

    /// <summary>
    /// The code in the start sets the cursor to the center and makes it invisible (like in other FPS games)
    /// </summary>
    // Start is called before the first frame update
    private void Start()
    {
        //Locks the cursor to the center
        Cursor.lockState = CursorLockMode.Locked;
        
        //getComponent for the camera
        camTransform = this.transform;
    }

    /// <summary>
    /// The code in the update sets the camera rotation based on the Mouse X and Y inputs. 
    /// It also sets the X rotation of the player to the same as the camera so the the movement controls match.
    /// </summary>
    // Update is called once per frame
    private void Update()
    {
        //sets the cursor to always visible
        Cursor.visible = true;
        //updates the X and Y Camera movement positions --> the yaw one subtracts or you get inverted controls for mouse y
        cameraPitch += cameraSpeedX * Input.GetAxis("Mouse X");
        cameraYaw -= cameraSpeedY * Input.GetAxis("Mouse Y");
        
        //clamp for look limit on the the Y axis. We do not need to do this to the x variable as
        //we need free 360 degree movement on the horizontal plane. We need to limit Y so we do not see the player or loop around too far on the y axis and get disoriented 
        cameraYaw = Mathf.Clamp(cameraYaw, yawLimitUpper, yawLimitLower);

        //this set the camera transform to the new Vector3 (camera positions that get updated). Euler angles is for 3D rotation
        camTransform.eulerAngles = new Vector3(cameraYaw, cameraPitch, 0);

        //get a local reference of the player position to be more efficient.
        var playerPositionLocal = playerTransform.position;
        
        //code for the camera to follow the player.This removed some negative behaviours when it came to rotations and positioning
        //player.transform.position.y + 2f is for the camera to be adjusted to the player height
        camTransform.position = new Vector3(playerPositionLocal.x,playerPositionLocal.y + 2f, playerPositionLocal.z);
    }
}
