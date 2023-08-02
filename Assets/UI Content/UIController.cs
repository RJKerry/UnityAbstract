using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    // Define the array to hold 3 X positions
    private float[] xPositions = new float[3];
    private GameObject selectionBox;
    private GameObject player;
    private List<ItemData> items;
    private ItemManager itemManager;
    void Start()
    {
        // Assign values to the array elements
        xPositions[0] = 370f;
        xPositions[1] = 570f;
        xPositions[2] = 770f;
        selectionBox = GameObject.FindGameObjectWithTag("SelectionBox");
        player = GameObject.Find("Player");
        itemManager = player.GetComponent<ItemManager>();
        items = itemManager.ownedItems;
        getItems();
    }


    // Get items from itemanager and display them in the UI

    public void getItems()
    {
        // Loop through the items list and display them in the UI
        for (int i = 0; i < items.Count; i++)
        {
            // Check if the item is not null
            if (items[i] != null)
                // Check if the item has an icon
                if (items[i].itemIcon == null)
                {
                    GameObject.Find("Slot-" + (i + 1)).transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                }
                // If it does, display it
                else
                {
                    GameObject.Find("Slot-" + (i + 1)).transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                    GameObject.Find("Slot-" + (i + 1)).transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = items[i].itemIcon;
                }
            // If the item is null, hide the slot
            else
            {
                GameObject.Find("Slot-" + (i + 1)).transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = new Color(1,1,1,0);
            }
        }

    }

    public void selectSlot(int index)
    {
        // Change the current item index in the item manager
        if (selectionBox != null)
        {
            // Change the current item index in the item manager
            selectionBox.transform.localPosition = new Vector3(xPositions[index], -380f, -6f);
        }


    }

}

