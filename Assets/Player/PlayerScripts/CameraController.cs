/*
    //***NOTE: This code is modified from the COMP396 classwork examples***

    Author's Name: Alexander  Maynard
    Creation Date: October 26, 2023
    Last Modified By: Alexander Maynard
    Last Modified Date: October 26, 2023
    Program Description: This script handles the camera movement of the player and rotation of the player object as well (to make the camera and player X rotations the same).
    Revision History:   
    -October 26, 2023
        -> Added firstvariables.
        -> Added list of ressources used to help make the camera turn with mouse imput.
        -> Added code in the start to lock the cursor and make it invisible.
        -> Added code in the update to move the camera X and Y using the camera speed.
        -> Added code to pair the player object and camera X rotations together.
 */

using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //*** NOTE: Ressources used to learn on how to move the camera with mouse input:
    // https://discussions.unity.com/t/setting-main-camera-rotation/43221
    // https://gamedev.stackexchange.com/questions/104693/how-to-use-input-getaxismouse-x-y-to-rotate-the-camera
    // https://forum.unity.com/threads/how-to-lock-or-set-the-cameras-z-rotation-to-zero.68932/#post-441968
    // https://discussions.unity.com/t/how-do-i-move-a-camera-with-mouse/194032/3


    //Reference to the player object
    public GameObject player;

    //cameraX and Y speeds
    public float cameraSpeedX = 4;
    public float cameraSpeedY = 4;
    
    //values for camera X and Y positions
    float cameraPitch = 0.0f;
    float cameraYaw = 0.0f;

    public float yawLimitUpper = 0;
    public float yawLimitLower = 0;

    /// <summary>
    /// The code in the start sets the cursor to the cneter and makes it invisible
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        //sets the cursor to the center and makes the cursor not visible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    /// <summary>
    /// The code in the update sets the camera rotation based on the MOuse X and Y inputs. It also sets the X rotaion of the player to the same as the camera so the the movement controls match.
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        //updates the X and Y Camera movement positions
        cameraPitch += cameraSpeedX * Input.GetAxis("Mouse X");
        //this one substracts or the up and dowwn movement is inverted for the controls
        cameraYaw -= cameraSpeedY * Input.GetAxis("Mouse Y");


        
        //clamp for look limit on the the Y axis. We do not need to do this to the x variable as
        //we need free 360 degree movement on the hgorizontal plane. We need to limit Y so we do not see the player or loop around too far on the y axis and get disoriented 
        cameraYaw = Mathf.Clamp(cameraYaw, yawLimitUpper, yawLimitLower);

        //this set the camera transform to the new Vecto3 (cameram positions that get updated). Euluer angles is for 3D rotation
        this.transform.eulerAngles = new Vector3(cameraYaw, cameraPitch, 0);



        //sets the X rotaion of the player to the same as the camera so the the movement controls match.
        player.transform.rotation = Quaternion.Euler(player.transform.position.x, cameraYaw, player.transform.position.z);
    }
}
