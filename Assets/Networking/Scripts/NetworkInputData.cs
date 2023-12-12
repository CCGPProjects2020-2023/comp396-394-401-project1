/*
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 12, 2023
    Program Description:    Input data to be considered on the network
    Revision History:       December 12, 2023: Initial script and documentation.                            
 */
//***The following is based on this tutorial:   https://www.youtube.com/watch?v=KqpMOdPj3co&list=PLyDa4NP_nvPfHhPuumJylSj8jXyULsT1X&index=1YouTube
using Fusion;
using UnityEngine;


public struct NetworkInputData : INetworkInput
{
    public Vector2 movementInput;
    public Vector3 aimForwardVector;
    public NetworkBool isJumpPressed;
    public NetworkBool isFirePressed;
    public NetworkBool isProjectilePressed;
}
