using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/ItemBase/TestItem")]
public class TestItem : ItemData
{
    public override void ItemUsed()
    {
        Debug.Log("ItemUsed called for" + Name);
    }
}