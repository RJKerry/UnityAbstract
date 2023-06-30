using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemBase")]
public abstract class ItemData : ScriptableObject
{
    public string Name = "Name";
    public Sprite itemIcon;
    public GameObject itemObject;
    //Reference to item UI icon - will
    public abstract void ItemUsed();
}