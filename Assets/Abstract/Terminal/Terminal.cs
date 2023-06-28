using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour, IInteractable, PlayerControls.ITerminalInputActions
{
    public CinemachineVirtualCamera terminalCam;
    public GameObject standPosition;

    public PlayerControls terminalControls;

    public PlayerInputManager interactingPlayer;

    private void Awake()
    {
        terminalControls = new PlayerControls();
        terminalControls.TerminalInput.SetCallbacks(this);

        standPosition = transform.GetChild(1).transform.gameObject;
        terminalCam = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    public void OnInteract(PlayerInputManager messageSource)
    {
        interactingPlayer = messageSource;
        terminalCam.Priority = 50; //TEST USE CAMERA SWITCHER LATER
        messageSource.canRecieveInput = false;
        messageSource.TeleportTo(standPosition.transform.position);
    }

    public void EndInteract()
    {
        interactingPlayer.canRecieveInput = true;
        terminalCam.Priority = 0;
        interactingPlayer = null;
    }

    public void OnExit(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(context.performed && interactingPlayer != null)
            EndInteract();
    }

    #region Input Enable/Disable
    private void OnEnable()
    {
        terminalControls.Enable();
    }

    private void OnDisable()
    {
        terminalControls.Disable();
    }
    #endregion
}