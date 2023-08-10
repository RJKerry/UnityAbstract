using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

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

    public TerminalCanvasController Screen;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        TerminalControls = new PlayerControls();
        TerminalControls.TerminalInput.SetCallbacks(this);

        TerminalCam = GetComponentInChildren<CinemachineVirtualCamera>();

        TerminalCanvas = GetComponentInChildren<Canvas>();
        TerminalCanvas.worldCamera = Camera.main; //This will work for instances in a single scene
        
        StandPosition = transform.GetChild(1).transform.gameObject;

        ButtonTemplate = Resources.Load<GameObject>("MenuAssets/ButtonGenBase");

        ActiveListeners = new Dictionary<ITerminalListener, Button>();

        Screen = GetComponentInChildren<TerminalCanvasController>();
    }

    /// <summary>
    /// Gathers references to objects implementing the ITerminalListener interface
    /// and appends them to a Dictionary of active listeners, and a reference to the button
    /// object returned by the GenerateInteractionButton
    /// </summary>
    public virtual void GatherTerminalListeners()
    {
        var listeners = FindObjectsOfType<MonoBehaviour>().OfType<ITerminalListener>();
        foreach (var listener in listeners) 
        {
            if (listener.IDGroup == IDGroup)
            {
                Debug.Log(listener.GetType());
                ActiveListeners.Add(listener, GenerateInteractionButton(listener));
            }
        }
    }

    /// <summary>
    /// Uses a template button to generate a button on the terminal
    /// based on the properties and fields within a given listener
    /// </summary>
    /// <param name="listener">The interface instance to access the fields of</param>
    /// <returns>A Button Object, referenced from a gameobject generated prior</returns>
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
        if (InteractingPlayer != null) //Already hooked into a player? Do not register new interacts.
            return;

        Screen.Activated(); //Currently a sprite sequence

        GatherTerminalListeners();

        RuntimeManager.PlayOneShot(PowerOn, transform.position);

        messageSource.playerControls.Disable();
        TerminalControls.TerminalInput.Enable();

        SetCameraPriority(50);
        
        messageSource.TeleportTo(StandPosition.transform.position); //Refactor to moveto
        
        //messageSource.canRecieveInput = false;
        InteractingPlayer = messageSource;
    }

    public void OnExit(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(context.performed && InteractingPlayer != null) {
            Debug.Log(gameObject.name);
            StartCoroutine(EndInteract());
        }
    }

    public IEnumerator EndInteract()
    {
        ClearListeners();
        Screen.Deactivate();
        
        RuntimeManager.PlayOneShot(PowerOff, transform.position);

        InteractingPlayer.playerControls.Enable();
        TerminalControls.TerminalInput.Disable();


        SetCameraPriority(0); //

        yield return new WaitForSecondsRealtime(DetachBuffer); //Prevents player moving preemptive to the camera switchback

        InteractingPlayer.canRecieveInput = true;
        InteractingPlayer = null;
        
    }

    public void SetCameraPriority(int priority)
    {
        TerminalCam.Priority = priority;
    }

    private void ClearListeners()
    { 
        foreach(KeyValuePair<ITerminalListener, Button> Listener in ActiveListeners)
        {
            Destroy(Listener.Value.gameObject); //Destroy the GameObject with Button Component
        }
        ActiveListeners.Clear();
    }

    #region Input Enable/Disable
    private void OnEnable()
    {
        TerminalControls.Enable();
    }

    private void OnDisable()
    {
        TerminalControls.Disable();
    }
    #endregion
}