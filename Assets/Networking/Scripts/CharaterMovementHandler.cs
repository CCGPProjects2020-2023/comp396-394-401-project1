using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class CharaterMovementHandler : NetworkBehaviour
{
    NetworkCharacterControllerPrototypeCustom networkCharacterControllerPrototypeCustom;

    Vector2 viewInput;

    float cameraRotationX = 0;
    Camera localCamera;

    private void Awake()
    {
        networkCharacterControllerPrototypeCustom = GetComponent<NetworkCharacterControllerPrototypeCustom>();
        localCamera = GetComponentInChildren<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cameraRotationX += viewInput.y * Time.deltaTime * networkCharacterControllerPrototypeCustom.viewUpDownRotationSpeed;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -90, 90);

        localCamera.transform.localRotation = Quaternion.Euler(cameraRotationX, 0, 0);
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData networkInputData)) {
            networkCharacterControllerPrototypeCustom.Rotate(networkInputData.rotationInput);

            Vector3 moveDirection = transform.forward * networkInputData.movementInput.y + transform.right * networkInputData.movementInput.x;
            moveDirection.Normalize();

            networkCharacterControllerPrototypeCustom.Move(moveDirection);

            if(networkInputData.isJumpPressed)
            {
                networkCharacterControllerPrototypeCustom.Jump();
            }
        }
    }

    public void SetViewInputVector(Vector2 viewinput) { 
        viewInput = viewinput;
    }
}
