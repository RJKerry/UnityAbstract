using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemBase/AccessCard")]

public class AccessCard : ItemData
{
    //This is where things that happen in the game world go
    public override void ItemUsed()
    {
        Debug.Log("Imagine some swiping happening right here");
    }
}