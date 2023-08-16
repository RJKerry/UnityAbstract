using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using FMODUnity;

/// <summary>
/// Terminal the functionality behind Terminal GameObjects in the world.
/// </summary>
public class Terminal : MonoBehaviour, IInteractable, PlayerControls.ITerminalInputActions
{

    public CinemachineVirtualCamera TerminalCam; //The camera position the terminal will be viewed from when used
    public GameObject StandPosition; //A gameobject marking the position the player object should be moved to upon interacting

    private PlayerControls TerminalControls; //Event Driven input reciever for the terminal
    public PlayerInputManager InteractingPlayer; //The player interating, if there is one

    public Canvas TerminalCanvas; //The Screen of the Terminal, driven by
    public TerminalCanvasController Screen; //A terminal canvas controller
    private Dictionary<ITerminalListener, Button> ActiveListeners; //listener references and generated buttons
    public int IDGroup = 0; //ID Groups to filter Listeners and terminals while referencing

    public GameObject ButtonTemplate; //The base button to generate for a listener

    public float DetachBuffer = 2f; //How long should be delayed before re-enabling the player's controls

    /// <summary>
    /// Directories to FMOD sound effects
    /// </summary>
    public string
        PowerOn = "event:/Terminal/PowerOn",
        PowerOff = "event:/Terminal/DoorUnlock/PowerOff";

    private void Awake()
    {
        Init();
    }

    /// <summary>
    /// Base setup for terminal object and its respective components
    /// </summary>
    private void Init()
    {
        TerminalControls = new PlayerControls();
        TerminalControls.TerminalInput.SetCallbacks(this);

        TerminalCam = GetComponentInChildren<CinemachineVirtualCamera>();
        if (TerminalCam == null)
            Debug.LogError("No CinemachineVirtualCamera found in object children");

        TerminalCanvas = GetComponentInChildren<Canvas>();
        if (TerminalCanvas == null)
            Debug.LogError("Terminals Should have a canvas within its children");

        TerminalCanvas.worldCamera = Camera.main; //This will work for instances in a single scene
        if (TerminalCanvas.worldCamera == null)
            Debug.LogError("Terminal has not found a camear to hook; buttons will not respond to mouse input");

        Screen = TerminalCanvas.gameObject.GetComponent<TerminalCanvasController>();
        if (Screen == null)
            Debug.LogError("Terminal should have a TerminalCanvasController");

        StandPosition = transform.Find("StandPosition").gameObject;
        if (StandPosition == null)
            Debug.LogError("Terminal should have a child named StandPosition");

        ButtonTemplate = Resources.Load<GameObject>("MenuAssets/ButtonGenBase");
        if (ButtonTemplate == null)
            Debug.LogError("No base button prefab found in resources");

        ActiveListeners = new Dictionary<ITerminalListener, Button>();
    }

    /// <summary>
    /// When the OnInteract interface Message is recieved from a valid source
    /// Trigger Terminal intro events & pass control to the terminal
    /// </summary>
    /// <param name="messageSource"></param>
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

        messageSource.TeleportTo(StandPosition.transform.position); //Refactor to smooth moveto

        InteractingPlayer = messageSource;
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

        GeneratedButton.onClick.AddListener(listener.OnActivated);
        
        return GeneratedButton;
    }


    /// <summary>
    /// Input Event from the ITerminalInputActions to trigger ending use of the terminal
    /// </summary>
    /// <param name="context"></param>
    public void OnExit(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(context.performed && InteractingPlayer != null) {
            StartCoroutine(EndInteract());
        }
    }

    /// <summary>
    /// Similar to OnInteract, except reverting control back to the player and preventing input to itself
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Disposes of Listeners as the player leaves the interaction
    /// </summary>
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