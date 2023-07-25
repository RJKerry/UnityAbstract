using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/ItemBase/TestItem")]
public class TestItem : ItemData
{
    public override void ItemUsed()
    {
        RaycastHit hitObject;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitObject, InteractDist))
        {
            Debug.Log(hitObject.transform.gameObject.name  + " raycast has been done");
        }
        Debug.Log("ItemUsed called for" + Name);
    }
}