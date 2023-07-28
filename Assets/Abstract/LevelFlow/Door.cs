using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, ITerminalListener, IInteractable
{
    public Vector3 StartPos, EndPos;
    public bool triggered;

    public bool Unlocked;

    float eTime, tTime = 1; //elapsed time, target time
    public Light LockedLight, UnlockedLight;

    public int _IDGroup; //Nescesarry for anything implementing ITerminalListener - Trigger ID Group
    public int IDGroup { get => _IDGroup; set => _IDGroup = value; } //Better syntax

    private void Awake()
    {
        StartPos = transform.position;
        EndPos = transform.position + Vector3.up * 5;
    }

    /// <summary>
    /// Update driven lerp is not a good thing, but this is for testing 
    /// </summary>
    void Update()
    {

    }

    public IEnumerator Activated(Vector3 startPos, Vector3 endPos)
    {
        if (true)
        {
            while (eTime < tTime)
            {
                transform.position = Vector3.Lerp(StartPos, EndPos, eTime / tTime);
                eTime += Time.deltaTime;
                yield return null;
            }
        }
    }

    public void OnActivated()
    {
        if (!Unlocked)
        {
            Debug.Log("I am a door");
            //Unlocked = true;
        } 
    }

    public void OnInteract(PlayerInputManager messageSource)
    {
        if (Unlocked) UnlockSequence();
    }

    public void UnlockSequence()
    {
        LockedLight.gameObject.SetActive(false);
        UnlockedLight.gameObject.SetActive(true);

    }
}