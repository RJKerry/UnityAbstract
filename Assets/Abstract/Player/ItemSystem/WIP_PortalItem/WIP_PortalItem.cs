using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/ItemBase/WIP_PortalItem")]
public class WIP_PortalItem : ItemData
{
    public override void ItemUsed()
    {
        Debug.Log("I Am portal item. I make looking holes");
    }
}
