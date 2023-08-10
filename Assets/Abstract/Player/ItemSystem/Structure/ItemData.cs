using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Data structure for items accessible within the game,
/// makes accessing relevant fields simple
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/ItemBase")]
public abstract class ItemData : ScriptableObject
{
    public string Name = "Name";
    public Sprite itemIcon;
    public GameObject itemObject;
    public float InteractDist;
    public abstract void ItemUsed();
}