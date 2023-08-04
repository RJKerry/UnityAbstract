using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.UIElements;
using UnityEditor.Events;
using UnityEngine.Events;
using FMOD;

public class Terminal : MonoBehaviour, IInteractable, PlayerControls.ITerminalInputActions
{

    public CinemachineVirtualCamera TerminalCam;
    public GameObject StandPosition;

    public PlayerControls TerminalControls;
    public PlayerInputManager InteractingPlayer;

    public Canvas TerminalCanvas;

    public int IDGroup = 0;
    private Dictionary<ITerminalListener, Button> ActiveListeners;

    public GameObject ButtonTemplate;

    public float DetachBuffer = 2f;

    public string
        PowerOn = "event:/Terminal/PowerOn",
        PowerOff = "event:/Terminal/DoorUnlock/PowerOff";

    private void Awake()
    {
        TerminalControls = new PlayerControls();
        TerminalControls.TerminalInput.SetCallbacks(this);

        StandPosition = transform.GetChild(1).transform.gameObject;
        TerminalCam = GetComponentInChildren<CinemachineVirtualCamera>();

        //set the terminal canvas' event camear to main camera somewhere otherwise assets wont drag and drop 

        TerminalCanvas = GetComponentInChildren<Canvas>();

        TerminalCanvas.worldCamera = Camera.main; //This will work for instances in a single scene

        ButtonTemplate = Resources.Load<GameObject>("MenuAssets/ButtonGenBase");

        ActiveListeners = new Dictionary<ITerminalListener, Button>();
        GatherTerminalListeners();
    }

    public virtual void GatherTerminalListeners()
    {
        var listeners = FindObjectsOfType<MonoBehaviour>().OfType<ITerminalListener>();
        foreach (var listener in listeners) 
        {
            if (listener.IDGroup == IDGroup)
            {
                UnityEngine.Debug.Log(listener.GetType());
                ActiveListeners.Add(listener, GenerateInteractionButton(listener));
            }
        }
    }

    public Button GenerateInteractionButton(ITerminalListener listener) //Could load a prefab from resources or create a whole new button 
    {
        GameObject GeneratedButtonObject = Instantiate(ButtonTemplate, Vector3.zero, Quaternion.identity);
        GeneratedButtonObject.transform.SetParent(TerminalCanvas.transform, false);
        Button GeneratedButton = GeneratedButtonObject.GetComponent<Button>();
        GeneratedButton.image.sprite = listener.TerminalButtonIcon;

        //UnityEventTools.AddPersistentListener(GeneratedButton.onClick, new UnityAction(listener.OnActivated));

        GeneratedButton.onClick.AddListener(listener.OnActivated);
        
        return GeneratedButton;
    }

    public void OnInteract(PlayerInputManager messageSource)
    {
        messageSource.playerControls.Disable();
        FMODUnity.RuntimeManager.PlayOneShot(PowerOn, transform.position);
        TerminalControls.TerminalInput.Enable();
        SetCameraPriority(50);
        messageSource.TeleportTo(StandPosition.transform.position);
        messageSource.canRecieveInput = false;
        UnityEngine.Debug.Log(gameObject.name);
        InteractingPlayer = messageSource;
    }


    public void SetCameraPriority(int priority)
    {
        TerminalCam.Priority = priority;
    }

    public IEnumerator EndInteract()
    {
        SetCameraPriority(0);
        FMODUnity.RuntimeManager.PlayOneShot(PowerOff, transform.position);
        yield return new WaitForSecondsRealtime(DetachBuffer); //Prevents player moving preemptive to the camera switchback
        InteractingPlayer.canRecieveInput = true;
        InteractingPlayer = null;
    }

    public void OnExit(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(context.performed && InteractingPlayer != null) {
            UnityEngine.Debug.Log(gameObject.name);
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