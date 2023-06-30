using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, ITerminalTriggerable
{
    public Vector3 StartPos, EndPos;
    public bool triggered;

    float eTime, tTime = 1; //elapsed time, target time

    private void Awake()
    {
        StartPos = transform.position;
        EndPos = transform.position+Vector3.up*5;
    }

    public void OnTriggered()
    {
        triggered = true;
    }

    /// <summary>
    /// Update driven lerp is not a good thing, but this is for testing 
    /// </summary>
    void Update()
    {
        if (triggered)
        {
            if (eTime < tTime)
            {
                transform.position = Vector3.Lerp(StartPos, EndPos, eTime / tTime);
                eTime += Time.deltaTime;
            }
        }
    }
}