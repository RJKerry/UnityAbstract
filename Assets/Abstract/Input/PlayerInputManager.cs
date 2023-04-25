using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerInputManager : MonoBehaviour, PlayerControls.IDefaultPlayerActions
{
    public GameObject cameraObject; //the gameobject the cinemachine brain is attached to
    public CharacterController playerCharacterController;
    public PlayerControls playerControls;
    public ItemManager playerItemManager;
    
    public Vector3 rawInputDirection; //Raw directional input translated to world space

    [SerializeField] private Vector3 cameraForward;
    
    public float movementSpeed = 5f;
    public float mouseSpeed = 100f;

    private float xRot = 0f, yRot = 0f;

    public int interactDist = 5;
    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.DefaultPlayer.SetCallbacks(this);

        playerCharacterController = GetComponent<CharacterController>();
        cameraObject = GetComponentInChildren<CinemachineVirtualCamera>().gameObject;
    }

    private void Update()
    {
        ApplyMovement();
    }
    private void ApplyMovement()
    {
        Vector3 relativeMovement = transform.forward * rawInputDirection.z + transform.right * rawInputDirection.x;
        playerCharacterController.Move( movementSpeed * Time.deltaTime * relativeMovement);
        if (!playerCharacterController.isGrounded) playerCharacterController.Move(Physics.gravity);
    }
    public void OnWalk(InputAction.CallbackContext context)
    {
        rawInputDirection = Vector3.zero;
        if (context.performed)
        {
            Vector2 input = context.ReadValue<Vector2>();
            rawInputDirection = new Vector3(input.x, 0, input.y);
            //Vector3 projectedInput = Vector3.ProjectOnPlane(new Vector3(input.x, 0, input.y), Vector3.up);
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 mouseDelta = context.ReadValue<Vector2>() * mouseSpeed * Time.deltaTime;
            xRot -= mouseDelta.y;
            yRot += mouseDelta.x;

            xRot = Mathf.Clamp(xRot, -90f, 90f);

            //gameObject.transform.Rotate(transform.up, mouseX * Time.deltaTime * mouseSpeed);
            //cameraObject.transform.Rotate(cameraObject.transform.right, -mouseY*Time.deltaTime*mouseSpeed, Space.World);

            transform.eulerAngles = new Vector3(0, yRot, 0);
            cameraObject.transform.localEulerAngles = new Vector3(xRot, 0, 0);
        }
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Interact key pressed");
    }
    public void OnUseItem(InputAction.CallbackContext context)
    {
        if(context.performed) playerItemManager?.UseItem();
    }
    
    #region Input Enable/Disable
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
    #endregion
}
