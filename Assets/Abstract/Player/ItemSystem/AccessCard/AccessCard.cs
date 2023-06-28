using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemBase/AccessCard")]

public class GrabObject : ItemData
{
    //This is where things that happen in the game world go
    public override void ItemUsed()
    {

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            ICanBeGrabbed grabbable = hit.collider.GetComponent<ICanBeGrabbed>();
            if (grabbable != null)
            {
                grabbable.OnGrabbed(hit.point);
            }
        }

    }
}