using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FloorItem can be attached to objects to make them give the player an item
/// when interacted with (i.e. picked up/obtained etc)
/// </summary>
public class FloorItem : MonoBehaviour, IInteractable
{
    public ItemData item;

    public void OnInteract(PlayerInputManager messageSource)
    {
        if (messageSource.playerItemManager.AddItem(item))
        {
            Destroy(gameObject);
        }
    }
}