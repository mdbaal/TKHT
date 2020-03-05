using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOBJ : MonoBehaviour
{
    public Item item;

    private void Awake()
    {
        this.name = item.name;
        this.GetComponent<SpriteRenderer>().sprite = item.sprite;
    }
}
