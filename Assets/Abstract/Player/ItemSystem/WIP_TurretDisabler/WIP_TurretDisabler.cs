using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemBase/WIP_TurretDisabler")]

public class WIP_TurretDisabler : ItemData
{
    public override void ItemUsed()
    {
        Debug.Log("That Turret Got Disabled");
    }
}