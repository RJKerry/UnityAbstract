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
        Debug.Log("OnGround?: " + playerCharacterController.isGrounded);
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
            
            /*float currentXRot = transform.localEulerAngles.x; //current rotation
            float deltaRot = currentXRot+mouseY; //the next rotation it wants to be
            float resultantCheck = deltaRot - currentXRot; //1 if its mouse up moving up or mouse down moving down, 0 otherwise (doesnt need to check. yay optimisation)
            float clampVal = 90; //this is gonna be 90 either way. if we have a negative value going down we invert it before the check
            float clampScaleCheck = mouseY + 1 % mouseY; //does it return a number above 1 
            Debug.Log(resultantCheck);*/

            
            /*//This can be reevaluated. Case 1 and -1 have the means to have the same calculation. if either one is valid do the same thing
            //else
            switch (resultantCheck)
            {
                case 1: //resultant check case 1 evaluates (Rotation+1)-Rotation +ve mouseDelta Y && +ve starting rotation i.e. +ve up
                    /*if (!deltaRot - clampVal < 1) //if, while having a positive rot with a positive delta, the rotation-clamp max is !<1, it needs clamping
                        mouseY = 0;#1#
                    break;
                case -1: // -ve mouse delta && -ve starting position
                    /*if (!deltaRot + clampVal < 1)
                        mouseY = 0;#1#
                    break;
                default: // Do nothing if it doesnt fall within these categories
                    
                    break;
            }*/

            //-0.5 + 1 = 0.5    0.5/-0.5 
            //if we wanna do the clamping were gonna need to either make our own or come up with a different approach we know works, so changing
            //how we apply and calculate the X rotation should allow us to clamp it. Rather than add the increment each time, add the increment to the value first,
            //then check its within the clamp range, and if not, clamp it
            
            //float rot = cameraObject.transform.eulerAngles.x+(mouseY*Time.deltaTime*mouseSpeed);
            //cameraObject.transform.eulerAngles.x = Mathf.Clamp(rot, -89f, 89f);

            
            //Vector3 cameraRotation = cameraObject.transform.localEulerAngles;
            
            //cameraRotation.x = Mathf.Clamp(cameraRotation.x - mouseY * Time.deltaTime * mouseSpeed, -89f, 89);

            gameObject.transform.Rotate(transform.up, mouseX * Time.deltaTime * mouseSpeed);
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
