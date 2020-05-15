using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOBJ : MonoBehaviour
{
    public Item item;

    public int droppedIndex = -1;

    private void Awake()
    {
        this.name = item.name;
        this.GetComponent<SpriteRenderer>().sprite = item.sprite;
    }
}
