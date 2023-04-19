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
    public float movementSpeed; //This could be a floating point on a curve as it evaluates, giving us fine control over dynamic acceleration
    void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.DefaultPlayer.SetCallbacks(this);
        
        playerCharacterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<CinemachineVirtualCamera>().gameObject;
    }

    void Update()
    { 
        playerCharacterController.Move(movementSpeed*Time.deltaTime*frameInput);
    }

    public void OnWalk(InputAction.CallbackContext context)
    {
        frameInput = Vector3.zero;
        if (context.performed)
        {
            Vector2 input = context.ReadValue<Vector2>();
            frameInput = new Vector3(input.x, 0, input.y).normalized;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //frameInput = frameInput*playerCamera.transform.rotation
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