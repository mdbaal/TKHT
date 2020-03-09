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

    public void addToPlayerInventory(Sprite itemSprite)
    {

        foreach (GameObject i in inventorySlots)
        {
            Image img = i.GetComponent<Image>();
            if (img.sprite == null) {
                img.sprite = itemSprite;
                img.SetNativeSize();
                img.enabled = true;
                return;
            }
        }
    }
}
