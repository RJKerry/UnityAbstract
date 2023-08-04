using FMOD;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour, ITerminalListener, IInteractable
{
    public Vector3 ClosePos, OpenPos;

    public bool triggered;
    public bool unlocked = true;
    [SerializeField]    private bool transitioning = false;

    public float eTime, tTime = 1f; //elapsed time, target time //Storing etime here allows for pause resume functionality
    public Light LockedLight, UnlockedLight;

    public int _IDGroup; //Nescesarry for anything implementing ITerminalListener - Trigger ID Group
    public int IDGroup { get => _IDGroup; set => _IDGroup = value; } //Better syntax

    public Sprite _TerminalButtonIcon;
    public Sprite TerminalButtonIcon { get => _TerminalButtonIcon; set => _TerminalButtonIcon = value; }

    public float UnlockDelay = 5f;

    /// <summary>
    /// References to FMOD events
    /// </summary>
    public string 
        DoorOpen = "event:/Door/DoorOpen",
        DoorClose = "event:/Door/DoorClose", 
        DoorUnlock = "event:/Door/DoorUnlock";

    private void Awake()
    {
        ClosePos = transform.position;
        OpenPos = transform.position+Vector3.up*3;
    }

    public IEnumerator OpenDoor(Vector3 StartPos, Vector3 EndPos)
    {
        if (transitioning)
            yield break; //Cannot Occur if already in motion

        transitioning = true;

        for (int i = 0; i <= 1; i++) //This is temporary and should be developed further (Auto close)
        {
            eTime = 0;
            FMODUnity.RuntimeManager.PlayOneShot(i == 0 ? DoorOpen : DoorClose, transform.position);
            yield return new WaitForSecondsRealtime(0.4f); //works to offset audio issue, this will be fixed

            while (eTime < tTime)
            {
                transform.position = Vector3.Lerp(i == 0 ? StartPos : EndPos, i == 0 ? EndPos : StartPos, eTime / tTime);
                eTime += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSecondsRealtime(2f);
        }
        transitioning = false;
    }

    public void OnInteract(PlayerInputManager messageSource)
    {
        if (unlocked) StartCoroutine(OpenDoor(ClosePos, OpenPos));
    }

    public void OnActivated()
    {
        //Unlock
        if (!unlocked)
        {
            StartCoroutine(UnlockSequence());
        }
    }

    public IEnumerator UnlockSequence()
    {
        LockedLight.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(UnlockDelay);
        UnlockedLight.gameObject.SetActive(true);
        unlocked = true;
        FMODUnity.RuntimeManager.PlayOneShot(DoorUnlock, transform.position);
    }
}