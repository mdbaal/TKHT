using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Output scrollrect")]
    public ScrollRect textScroll;

    public List<GameObject> inventorySlots = new List<GameObject>();

    public List<GameObject> equipmentSlots = new List<GameObject>();

    public void toBottom()
    {
        Canvas.ForceUpdateCanvases();

        textScroll.verticalNormalizedPosition = 0;
    }

    public void addToPlayerInventory(Item item)
    {
        foreach (GameObject i in inventorySlots)
        {
            Image img = i.GetComponent<Image>();
           if (img.sprite == null) {
                img.sprite = item.sprite;
                img.enabled = true;
                return;
            }
        }
    }

    public void removeFromPlayerInventory(Item item)
    {

        foreach (GameObject i in inventorySlots)
        {
            Image img = i.GetComponent<Image>();

            if (img.sprite == item.sprite)
            {
                img.sprite = null;
                img.enabled = false;   
                return;
            }
        }
    }

    public void addToEquiped(Item item)
    {
        
        removeFromPlayerInventory(item);
        if (item.equipLeft)
        {
            Image img = equipmentSlots[0].GetComponentInChildren<Image>();
           img.sprite = item.sprite;
            img.enabled = true;
        }
        else
        {
            Image img = equipmentSlots[1].GetComponentInChildren<Image>();
            img.sprite = item.sprite;
            img.enabled = true;
        }
    }

    public void removeFromEquiped(Item item)
    {
        addToPlayerInventory(item);
        if (item.equipLeft)
        {
            Image img = equipmentSlots[0].GetComponentInChildren<Image>();
            img.sprite = null;
            img.enabled = false;
        }
        else
        {
            Image img = equipmentSlots[1].GetComponentInChildren<Image>();
            img.sprite = null;
            img.enabled = false;
        }
    }
}
