using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemManager : MonoBehaviour
{
    //reference to however we are communicating this data if we need one
    public ItemData equippedItem;
    public int itemIndex = 0;
    public List<ItemData> ownedItems; //This can be a dictionary at some point potentially
    private int maxItems = 3;

    public GameObject itemSocket;
    
    public ItemSlotManager slotManager;

    public void UseItem()
    {
        equippedItem?.ItemUsed();
    }

    private void Awake()
    {
        Init();
    }

    /// <summary>
    /// General setup for this object
    /// </summary>
    void Init()
    {
        ownedItems.Add(ScriptableObject.CreateInstance<TestItem>()); //TEMPORARY
        itemSocket = GameObject.FindGameObjectWithTag("ItemSocket"); 
        slotManager = FindObjectOfType<ItemSlotManager>();
        equippedItem = ownedItems[0];
        CycleItem(0, WeaponSwitchTypes.Absolute);
    }
    
    /// <summary>
    /// Cycles between items through steps or absolute values
    /// </summary>
    /// <param name="step">the value being used to modify the current item index</param>
    /// <param name="switchType">The kind of change you want to apply</param>
    public void CycleItem(int step, WeaponSwitchTypes switchType)
    {
        //Debug.Log(step);
        int prevIndex = itemIndex;
        switch (switchType)
        {
            case WeaponSwitchTypes.Absolute:
                if (step < ownedItems.Count && step >= 0)
                    itemIndex = itemIndex != step ? step : itemIndex; //if new != old update, else stay as old
                break;
            case WeaponSwitchTypes.Cycle:
                int newIndex = itemIndex + step; //will be limited to a normalized axis input (-1 to 1)
                if (!(newIndex < ownedItems.Count && newIndex >= 0)) //if the telegraphed new index is out of bounds
                {
                    if(newIndex < 0)
                        newIndex = ownedItems.Count-1;
                    if (newIndex >= ownedItems.Count)
                        newIndex = 0;
                }
                itemIndex = newIndex;
                break;
            default:
                Debug.LogError("WeaponSwitchType " + switchType + "Has no implemented case");
                break;
        }
        if(itemIndex!=prevIndex)
            UpdateItemData();
    }

    public void UpdateItemData()
    {
        equippedItem = ownedItems[itemIndex];
        //slotManager.ActiveUISlots[itemIndex].OnSelected();
        //Trigger item swap animation, or send a message to a respective animation manager (second would be nicer)
    }

    public bool AddItem(ItemData newItem)
    {
        if (ownedItems.Count < maxItems)
        {
            ownedItems.Add(newItem);
            return true;
        }
        return false;
    }

}

public enum WeaponSwitchTypes
{
    Cycle, //+1/-1 to the index
    Absolute //abs input index 0 to n where n is items the player has
}