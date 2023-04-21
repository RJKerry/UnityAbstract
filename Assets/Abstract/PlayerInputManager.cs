using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerInputManager : MonoBehaviour, PlayerControls.IDefaultPlayerActions
{
    public GameObject cameraObject;
    public CharacterController playerCharacterController;
    public PlayerControls playerControls;
    
    public Vector3 RawInputDirection;
    //public Vector3 RotatedInputDirection;
    
    [SerializeField] private Vector3 cameraForward;
    
    public float movementSpeed = 5f;
    public float mouseSpeed = 100f;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.DefaultPlayer.SetCallbacks(this);

        playerCharacterController = GetComponent<CharacterController>();
        cameraObject = GetComponentInChildren<CinemachineVirtualCamera>().gameObject;
    }

    private void Update()
    {
        //Apply movement to the player


        //cameraForward = cameraObject.transform.TransformDirection(Vector3.forward);
        //RotatedInputDirection = Quaternion.LookRotation(cameraForward, Vector3.up) * RawInputDirection;
        /*Vector3 fwd = transform.forward * RawInputDirection.z;
        Vector3 rgt = transform.right * RawInputDirection.x;*/
        
        
        Vector3 relativeMovement = transform.forward * RawInputDirection.z + transform.right * RawInputDirection.x;
        playerCharacterController.Move( movementSpeed * Time.deltaTime * relativeMovement);
        if (!playerCharacterController.isGrounded) playerCharacterController.Move(Physics.gravity);

    }

    public void OnWalk(InputAction.CallbackContext context)
    {
        RawInputDirection = Vector3.zero;
        if (context.performed)
        {
            Vector2 input = context.ReadValue<Vector2>();
            RawInputDirection = new Vector3(input.x, 0, input.y);
            //Vector3 projectedInput = Vector3.ProjectOnPlane(new Vector3(input.x, 0, input.y), Vector3.up);
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 mouseDelta = context.ReadValue<Vector2>();
            float mouseY = mouseDelta.y;
            float mouseX = mouseDelta.x;
            
            gameObject.transform.Rotate(transform.up, mouseX * Time.deltaTime * mouseSpeed);

            //Vector3 cameraRotation = cameraObject.transform.localEulerAngles;
            
            //cameraRotation.x = Mathf.Clamp(cameraRotation.x - mouseY * Time.deltaTime * mouseSpeed, -89f, 89);

            cameraObject.transform.Rotate(cameraObject.transform.right, -mouseY*Time.deltaTime*mouseSpeed, Space.World);
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
