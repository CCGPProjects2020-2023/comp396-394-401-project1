/**
    Author's Name: Alexander  Maynard
    Creation Date: October 27, 2023
    Last Modified By: Alexander Maynard
    Last Modified Date: October 27, 2023
    Program Description: This script handles the camera movement of the player and rotation of the player 
    object as well (to make the camera and player X rotations the same).
    Revision History:  
        -Ocotober 27, 2023:
            -> Transfered this code from CameraController
            -> modified the code added to work in this script and removed the start
            -> Added more comments
            -> Changed to FixedUpdate for more consistent Lerp to adjust player rotation.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class updates the player rotation on the y-axis equal to the y rotation of the camera so that WASD movement keys are updated accordingly.
/// </summary>
public class UpdatePlayerRotation : MonoBehaviour
{
    //Reference to the player camera
    public GameObject cam;

    // Update is called once per frame
    void FixedUpdate()
    {
        //New Quanterion that is updated to match the camera's Y and the players current other values as we do not want to update those
        Quaternion playerNewRotationX = new Quaternion(this.transform.rotation.x, cam.transform.rotation.y, this.transform.rotation.z, cam.transform.rotation.w);
        //this was used for help on solving quaternion only on y
        //sets the X rotaion of the player to the same as the camera so the the movement controls match. Time.deltaTime * 1000 controls the interpolation speed
        //Lerp is used here instead of slerp as the movement of the player is only on one line.
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, playerNewRotationX, 1);
    }
}
