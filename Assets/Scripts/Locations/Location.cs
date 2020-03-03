using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    public new string name;

    public string description;

    public Sprite sprite;
    private Inventory inventory = new Inventory();

    public Location[] neighbours;

    private void Awake()
    {
        makeLocation();
    }

    public bool hasNeighbour(string l)
    {
        foreach(Location L in neighbours)
        {
            if (l.Equals(L.name)) return true;
        }
        return false;
    }

    private void makeLocation()
    {
        Item[] items = this.GetComponentsInChildren<Item>();

        foreach(Item i in items)
        {
            inventory.addItem(i);
        }

        SpriteRenderer sceneImg = this.gameObject.AddComponent<SpriteRenderer>();

        sceneImg.sprite = this.sprite;
    }

    public Item takeItem(string item)
    {
        return inventory.getItem(item);
    }

    public void dropItem(Item item)
    {
        inventory.addItem(item);
    }

    public string getDescription()
    {
        return description;
    }

    public string listItems()
    {
        return inventory.ToString();
    }



}
