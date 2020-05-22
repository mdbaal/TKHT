using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOBJ : MonoBehaviour
{
    [SerializeField]
    private Item _item;

    [SerializeField]
    private int _droppedIndex = -1;

    public Item item { get => _item; set => _item = value; }
    public int droppedIndex { get => _droppedIndex; set => _droppedIndex = value; }

    private void Awake()
    {
        this.name = item.name;
        this.GetComponent<SpriteRenderer>().sprite = item.sprite;
    }
}
