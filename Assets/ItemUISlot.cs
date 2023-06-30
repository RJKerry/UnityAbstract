using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUISlot : MonoBehaviour
{
    Sprite itemIcon;

    void UpdateItemSlet(ItemData itemData) 
    {
        itemIcon = itemData.itemIcon;
    }

    public void OnSelected()
    {
        Debug.Log("WEEE");
    }
}
