using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour, PlayerControls.IDefaultPlayerActions
{
    public GameObject playerCamera;
    public CharacterController playerCharacterController;
    public PlayerControls playerControls;
    public Vector3 frameInput;
    public Vector3 cameraForward;
    public float movementSpeed;

    public float mouseSpeed = 100f;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.DefaultPlayer.SetCallbacks(this);

        playerCharacterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<CinemachineVirtualCamera>().gameObject;
    }

    private void Update()
    {
        cameraForward = playerCamera.transform.TransformDirection(Vector3.forward);
        playerCharacterController.Move(frameInput * movementSpeed * Time.deltaTime);
        if (!playerCharacterController.isGrounded) playerCharacterController.Move(Physics.gravity);

    }

    public void OnWalk(InputAction.CallbackContext context)
    {
        frameInput = Vector3.zero;
        if (context.performed)
        {
            Vector2 input = context.ReadValue<Vector2>();
            Vector3 planeNormal = Vector3.up;
            //Vector3 planePoint = transform.position;
            Vector3 projectedInput = Vector3.ProjectOnPlane(new Vector3(input.x, 0, input.y), planeNormal);
            frameInput = Quaternion.LookRotation(cameraForward, planeNormal) * projectedInput;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 mouseDelta = context.ReadValue<Vector2>();
            float mouseY = mouseDelta.y;
            float mouseX = mouseDelta.x;

            // Rotate the player object left and right based on the mouse movement
            gameObject.transform.Rotate(transform.up, mouseX * Time.deltaTime * mouseSpeed);

            Vector3 cameraRotation = playerCamera.transform.localEulerAngles;
            
            //cameraRotation.x = Mathf.Clamp(cameraRotation.x - mouseY * Time.deltaTime * mouseSpeed, -89f, 89);
            //cameraRotation.x = cameraRotation.x - mouseY * Time.deltaTime * mouseSpeed;
            //playerCamera.transform.localRotation = Quaternion.Euler(cameraRotation);
            playerCamera.transform.Rotate(playerCamera.transform.right, -mouseY*Time.deltaTime*mouseSpeed, Space.World);
            
            
            // Calculate a new movement vector based on the player's orientation
            Vector3 forward = playerCamera.transform.forward;
            forward.y = 0;
            forward.Normalize();
            Vector3 right = playerCamera.transform.right;
            right.y = 0;
            right.Normalize();
            frameInput = forward * playerControls.DefaultPlayer.Walk.ReadValue<Vector2>().y + right * playerControls.DefaultPlayer.Walk.ReadValue<Vector2>().x;
        }
    }



    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
