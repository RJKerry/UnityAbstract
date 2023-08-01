using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Terminal : MonoBehaviour, IInteractable, PlayerControls.ITerminalInputActions
{

    public CinemachineVirtualCamera TerminalCam;
    public GameObject StandPosition;

    public PlayerControls TerminalControls;
    public PlayerInputManager InteractingPlayer;

    public Canvas TerminalCanvas;

    public int IDGroup = 0;
    private Dictionary<ITerminalListener, Button> ActiveListeners;

    public float DetachBuffer = 2f;

    private void Awake()
    {
        TerminalControls = new PlayerControls();
        TerminalControls.TerminalInput.SetCallbacks(this);

        StandPosition = transform.GetChild(1).transform.gameObject;
        TerminalCam = GetComponentInChildren<CinemachineVirtualCamera>();

        //set the terminal canvas' event camear to main camera somewhere otherwise assets wont drag and drop 

        TerminalCanvas = GetComponentInChildren<Canvas>();

        ActiveListeners = new Dictionary<ITerminalListener, Button>();
        GatherTerminalListeners();
    }

    public virtual void GatherTerminalListeners()
    {
        List<ITerminalListener> listeners = (List<ITerminalListener>)FindObjectsOfType<MonoBehaviour>().OfType<ITerminalListener>();
        foreach (ITerminalListener listener in listeners) 
        {
            if (listener.IDGroup == IDGroup)
            {
                Debug.Log(listener);
                ActiveListeners.Add(listener, GenerateInteractionButton(listener));
            }
        }
    }
    public Button GenerateInteractionButton(ITerminalListener listener) //Could load a prefab from resources or create a whole new button 
    {
        Button GeneratedButton = gameObject.AddComponent<Button>();
        //button canvas parent
        GeneratedButton.transform.SetParent(TerminalCanvas.transform, false);
        GeneratedButton.image.sprite = listener.TerminalButtonIcon;
        GeneratedButton.onClick.AddListener(listener.OnActivated);
        return GeneratedButton;
    }

    public void OnInteract(PlayerInputManager messageSource)
    {
        messageSource.playerControls.Disable();
        TerminalControls.TerminalInput.Enable();
        SetCameraPriority(50);
        messageSource.TeleportTo(StandPosition.transform.position);
        messageSource.canRecieveInput = false;
        Debug.Log(gameObject.name);
        InteractingPlayer = messageSource;

        //TEMPORARY
/*        foreach (var listener in ActiveListeners)
        {
            //listener.OnActivated();
        }*/
    }


    public void SetCameraPriority(int priority)
    {
        TerminalCam.Priority = priority;
    }

    public IEnumerator EndInteract()
    {
        SetCameraPriority(0);
        yield return new WaitForSecondsRealtime(DetachBuffer); //Prevents player moving preemptive to the camera switchback
        InteractingPlayer.canRecieveInput = true;
        InteractingPlayer = null;
    }

    public void OnExit(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(context.performed && InteractingPlayer != null) {
            Debug.Log(gameObject.name);
            InteractingPlayer.playerControls.Enable();
            TerminalControls.TerminalInput.Disable();
            StartCoroutine(EndInteract());
        }
    }

    #region Input Enable/Disable
    private void OnEnable()
    {
        TerminalControls.Disable();
    }

    private void OnDisable()
    {
        OnEnable();
    }
    #endregion
}