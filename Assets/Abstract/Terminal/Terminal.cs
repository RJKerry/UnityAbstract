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

    public Door obj;

    private void Awake()
    {
        terminalControls = new PlayerControls();
        terminalControls.TerminalInput.SetCallbacks(this);

        standPosition = transform.GetChild(1).transform.gameObject;
        terminalCam = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    public virtual void GatherTerminalTriggers()
    {
        obj.OnTriggered();
    }

    public void OnInteract(PlayerInputManager messageSource)
    {
        messageSource.playerControls.Disable();
        terminalControls.TerminalInput.Enable();
        setPriority(50);
        messageSource.TeleportTo(standPosition.transform.position);
        messageSource.canRecieveInput = false;
        Debug.Log(gameObject.name);
        interactingPlayer = messageSource;
    }

    public void setPriority(int priority)
    {
        terminalCam.Priority = priority;
    }

    public void EndInteract()
    {
        setPriority(0);
        interactingPlayer.canRecieveInput = true;
        interactingPlayer = null;

    }

    public void OnExit(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(context.performed && interactingPlayer != null) {
            Debug.Log(gameObject.name);
            interactingPlayer.playerControls.Enable();
            terminalControls.TerminalInput.Disable();
            EndInteract();
        }
    }

    #region Input Enable/Disable
    private void OnEnable()
    {
        terminalControls.Disable();
    }

    private void OnDisable()
    {
        OnEnable();
    }
    #endregion
}