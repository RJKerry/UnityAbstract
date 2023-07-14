using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

//Movement functionalities
public class PlayerInputManager : MonoBehaviour, PlayerControls.IDefaultPlayerActions
{
    public GameObject cameraObject; //the gameobject the cinemachine brain is attached to
    public CharacterController playerCharacterController;
    public PlayerControls playerControls;
    public ItemManager playerItemManager;
    private PauseController playerPauseController;
    
    public Vector3 rawMovementInput; //Raw directional input translated to world space
    public Vector3 rawLookInput;

    [SerializeField] private Vector3 cameraForward;
    
    public float movementSpeed = 5f;
    public float mouseSpeed = 100f;

    private float xRot = 0f, yRot = 0f;

    private Vector2 lastViableControllerInput;

    public int interactDist = 5;

    public bool canRecieveInput = true;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (canRecieveInput)
        {
            ApplyMovement();
        }
        
        if (lastViableControllerInput.x != 0 || lastViableControllerInput.y != 0) // do not move if no direction, deadzone is handled within the input manager
                ApplyLook(lastViableControllerInput);
    }

    /// <summary>
    /// Initialises the components the input manager needs to access
    /// </summary>
    private void Init()
    {
        playerControls = new PlayerControls();
        playerControls.DefaultPlayer.SetCallbacks(this);

        playerCharacterController = GetComponent<CharacterController>();
        playerPauseController = GetComponent<PauseController>();
        playerItemManager = GetComponent<ItemManager>();
        cameraObject = GetComponentInChildren<CinemachineVirtualCamera>().gameObject;

    }

    /// <summary>
    /// Calculates the final movement direction of the character and applies it
    /// then if player is not on the ground it applies (technically not realistic)
    /// gravity.
    /// </summary>
    private void ApplyMovement()
    {
        Vector3 relativeMovement = transform.forward * rawMovementInput.z + transform.right * rawMovementInput.x;

        playerCharacterController.Move(movementSpeed * Time.deltaTime * relativeMovement);

        if (!playerCharacterController.isGrounded)
            playerCharacterController.Move(Physics.gravity * Time.deltaTime);
    }

    /// <summary>
    /// Applies the current look input to the camera and player
    /// </summary>
    /// <param name="LookDirection"></param>
    private void ApplyLook(Vector2 LookDirection) 
    {
        if (canRecieveInput)
        {
            xRot -= LookDirection.y;
            xRot = Mathf.Clamp(xRot, -90f, 90f);
            yRot += LookDirection.x;

            transform.eulerAngles = new Vector3(0, yRot, 0);
            cameraObject.transform.localEulerAngles = new Vector3(xRot, 0, 0);
        }
    }

    /// <summary>
    /// Reads the physical movement input and
    /// reformats it to Vector3 to represent world space
    /// </summary>
    /// <param name="context">A vector 2 input direction value based on WASD and Left Stick</param>
    public void OnWalk(InputAction.CallbackContext context)
    {
        rawMovementInput = Vector3.zero;
        if (context.performed)
        {
            Vector2 input = context.ReadValue<Vector2>();
            rawMovementInput = new Vector3(input.x, 0, input.y);
        }
    }


    /// <summary>
    /// Reads the mouse delta input and calls the
    /// ApplyLook method using it as a parameter
    /// </summary>
    /// <param name="context">A Vector2 Representing mouse delta direction</param>
    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 mouseDelta = context.ReadValue<Vector2>() * mouseSpeed * Time.deltaTime;

            ApplyLook(mouseDelta);
        }
    }

    /// <summary>
    /// Reads the stick direction input
    /// 
    /// Functionalitry for sticks differ slightly to the mouse
    /// It persistently applies as opposed to being triggered once
    /// </summary>
    /// <param name="context"></param>
    public void OnStickLook(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            lastViableControllerInput = Vector2.zero;
            return;
        }
        if (context.performed)
        {
            Vector2 stickDelta = context.ReadValue<Vector2>() * mouseSpeed * Time.deltaTime;
            if(stickDelta.x != 0)
                lastViableControllerInput.x = stickDelta.x;
            if(stickDelta.y != 0)
                lastViableControllerInput.y = stickDelta.y;
        }
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if(canRecieveInput && context.performed)
        {
            RaycastHit hitObject;
            if (Physics.Raycast(cameraObject.transform.position, cameraObject.transform.forward, out hitObject))
            {
                hitObject.transform.GetComponent<IInteractable>()?.OnInteract(this); //Condensed interface check
            }
        }
    }

    public void OnUseItem(InputAction.CallbackContext context)
    {
        if(context.performed) playerItemManager?.UseItem();
    }

    /// <summary>
    /// Reads a step value that is positive or negative
    /// depending on the intended scroll direction
    /// </summary>
    /// <param name="context">A floating point value between -1 and 1</param>
    public void OnCycleItem(InputAction.CallbackContext context)
    {
        if(context.performed) playerItemManager?.CycleItem((int)context.ReadValue<float>(), WeaponSwitchTypes.Cycle);
    }

    #region ItemKeys
    public void OnSelectSlot1(InputAction.CallbackContext context)
    {
        if(context.performed) playerItemManager.CycleItem(0, WeaponSwitchTypes.Absolute);
    }
    public void OnSelectSlot2(InputAction.CallbackContext context)
    {
        if(context.performed) playerItemManager.CycleItem(1, WeaponSwitchTypes.Absolute);
    }
    public void OnSelectSlot3(InputAction.CallbackContext context)
    {
        if(context.performed) playerItemManager.CycleItem(2, WeaponSwitchTypes.Absolute);
    }
    #endregion


    public void TeleportTo(Vector3 newPosition)
    { 
        gameObject.transform.position = newPosition;
    }


    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
            playerPauseController.TogglePause();
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