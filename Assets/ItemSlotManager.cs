using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ItemSlotManager : MonoBehaviour
{
    public List<ItemUISlot> ActiveUISlots;

    private void Awake()
    {
        foreach(ItemUISlot slot in GetComponentsInChildren<ItemUISlot>())
            ActiveUISlots.Add(slot);
    }
}
