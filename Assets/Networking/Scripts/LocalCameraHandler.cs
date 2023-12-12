/*
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 12, 2023
    Program Description:    Handles the local camera of the player
    Revision History:       December 12, 2023: Initial script and documentation.                            
 */
//***The following is based on this tutorial:   https://www.youtube.com/watch?v=KqpMOdPj3co&list=PLyDa4NP_nvPfHhPuumJylSj8jXyULsT1X&index=1YouTube
using UnityEngine;

public class LocalCameraHandler : MonoBehaviour
{
    public Transform cameraAnchorPoint;
    NetworkCharacterControllerPrototypeCustom networkCharacterControllerPrototypeCustom;

    Vector2 viewInput;

    float cameraRotationX = 0;
    float cameraRotationY = 0;

    public Camera localCamera;

    /// <summary>
    /// Awake method called by unity - Initializes properties
    /// </summary>
    private void Awake()
    {
        localCamera = GetComponent<Camera>();
        networkCharacterControllerPrototypeCustom = GetComponentInParent<NetworkCharacterControllerPrototypeCustom>();
    }

    /// <summary>
    /// Start method called by unity - Initializes properties
    /// </summary>
    void Start()
    {
        cameraRotationX = GameManager.instance.cameraViewRotation.x;
        cameraRotationY = GameManager.instance.cameraViewRotation.y;
    }

    /// <summary>
    /// LateUpdate method called by unity - Updates the camera rotation
    /// </summary>
    void LateUpdate()
    {
        if (cameraAnchorPoint == null) return;

        if(!localCamera.enabled) return;

        localCamera.transform.position = cameraAnchorPoint.position;

        cameraRotationX += viewInput.y * Time.deltaTime * networkCharacterControllerPrototypeCustom.viewUpDownRotationSpeed;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -90, 90);

        cameraRotationY += viewInput.x * Time.deltaTime * networkCharacterControllerPrototypeCustom.rotationSpeed;

        localCamera.transform.rotation = Quaternion.Euler(cameraRotationX, cameraRotationY, 0);
    }

    /// <summary>
    /// Sets the viewInput property
    /// </summary>
    /// <param name="viewInput"></param>
    public void SetViewInputVector(Vector2 viewInput) {
        this.viewInput = viewInput;
    }

    /// <summary>
    /// Sets the GameManager camera rotation to the cameraRotation properties of this class.
    /// </summary>
    private void OnDestroy()
    {
        if (cameraRotationX != 0 && cameraRotationY != 0) { 
            GameManager.instance.cameraViewRotation.x = cameraRotationX;
            GameManager.instance.cameraViewRotation.y = cameraRotationY;
        }
    }
}
