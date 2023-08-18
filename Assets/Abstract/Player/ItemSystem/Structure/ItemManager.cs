using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemManager : MonoBehaviour
{
    //reference to however we are communicating this data if we need one
    public int CurrentItemIndex = 0;
    public List<ItemData> ownedItems; //This can be a dictionary at some point potentially
    private int maxItems = 3;

    public GameObject itemSocket;

    public void UseItem()
    {
        ownedItems[CurrentItemIndex]?.ItemUsed();
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
        ownedItems.Add(ScriptableObject.CreateInstance<TestItem>()); //TEMPORARY -- ALSO MAKES A BLANK OBJECT, 
        itemSocket = GameObject.FindGameObjectWithTag("ItemSocket"); 
       // slotManager = FindObjectOfType<ItemSlotManager>();
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
        int prevIndex = CurrentItemIndex;
        switch (switchType)
        {
            case WeaponSwitchTypes.Absolute:
                if (step < ownedItems.Count && step >= 0)
                    CurrentItemIndex = CurrentItemIndex != step ? step : CurrentItemIndex; //if new != old update, else stay as old
                break;
            case WeaponSwitchTypes.Cycle:
                int newIndex = CurrentItemIndex + step; //will be limited to a normalized axis input (-1 to 1)
                if (!(newIndex < ownedItems.Count && newIndex >= 0)) //if the telegraphed new index is out of bounds
                {
                    if(newIndex < 0)
                        newIndex = ownedItems.Count-1;
                    if (newIndex >= ownedItems.Count)
                        newIndex = 0;
                }
                CurrentItemIndex = newIndex;
                break;
            default:
                Debug.LogError("WeaponSwitchType " + switchType + "Has no implemented case");
                break;
        }
        if(CurrentItemIndex!=prevIndex)
            SwitchToNewItem();
    }

    public void SwitchToNewItem()
    {
        if (ownedItems[CurrentItemIndex] != null)
        {
            GameObject equippedItem = Instantiate(ownedItems[CurrentItemIndex], Vector3.zero, Quaternion.identity, itemSocket.transform).GameObject();
        }
        
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