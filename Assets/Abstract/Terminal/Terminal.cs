using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Terminal : MonoBehaviour, IInteractable, PlayerControls.ITerminalInputActions
{

    public CinemachineVirtualCamera terminalCam;
    public GameObject standPosition;

    public PlayerControls terminalControls;
    public PlayerInputManager interactingPlayer;

    public int IDGroup = 0;
    private List<ITerminalListener> ActiveListeners;

    public float DetachBuffer = 2f;

    private void Awake()
    {
        terminalControls = new PlayerControls();
        terminalControls.TerminalInput.SetCallbacks(this);

        standPosition = transform.GetChild(1).transform.gameObject;
        terminalCam = GetComponentInChildren<CinemachineVirtualCamera>();

        //set the terminal canvas' event camear to main camera somewhere otherwise it wont drag and drop work

        ActiveListeners = new List<ITerminalListener>();
        GatherTerminalListeners();
    }

    public virtual void GatherTerminalListeners()
    {
        var listeners = FindObjectsOfType<MonoBehaviour>().OfType<ITerminalListener>();
        foreach (var listener in listeners) 
        {
            if (listener.IDGroup == IDGroup)
            {
                Debug.Log(listener);
                ActiveListeners.Add(listener);
            }
        }
    }

    public void OnInteract(PlayerInputManager messageSource)
    {
        messageSource.playerControls.Disable();
        terminalControls.TerminalInput.Enable();
        SetCameraPriority(50);
        messageSource.TeleportTo(standPosition.transform.position);
        messageSource.canRecieveInput = false;
        Debug.Log(gameObject.name);
        interactingPlayer = messageSource;

        //TEMPORARY
/*        foreach (var listener in ActiveListeners)
        {
            //listener.OnActivated();
        }*/
    }

    public void SetCameraPriority(int priority)
    {
        terminalCam.Priority = priority;
    }

    public IEnumerator EndInteract()
    {
        SetCameraPriority(0);
        yield return new WaitForSecondsRealtime(DetachBuffer); //Prevents player moving preemptive to the camera switchback
        interactingPlayer.canRecieveInput = true;
        interactingPlayer = null;
    }

    public void OnExit(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(context.performed && interactingPlayer != null) {
            Debug.Log(gameObject.name);
            interactingPlayer.playerControls.Enable();
            terminalControls.TerminalInput.Disable();
            StartCoroutine(EndInteract());
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