using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_GrabbableBoxPuzzle : MonoBehaviour, ICanBeGrabbed
{
    public Vector3 StartPos, EndPos;
    public bool triggered;

    float eTime, tTime = 1; //elapsed time, target time

    private void Awake()
    {
        StartPos = transform.position;
        EndPos = transform.position+Vector3.up*5;
    }

    public void OnGrabbed(Vector3 hitPoint)
    {
        triggered = true;
    }

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