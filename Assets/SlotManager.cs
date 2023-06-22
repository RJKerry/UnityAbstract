using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    public List<Image> icons;

    private void Awake()
    {
        InitReferences();
    }

    public void InitReferences() //References all of the icon slots in game for 
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("ItemSpriteSlot"); //Gathers relevantly tagged objects
        foreach (GameObject obj in objs)
        {
            icons.Add(obj.GetComponent<Image>()); //gets image component references
        }
    }
}