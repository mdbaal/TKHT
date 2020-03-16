using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Output scrollrect")]
    public ScrollRect textScroll;

    public List<GameObject> inventorySlots = new List<GameObject>();

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
}
