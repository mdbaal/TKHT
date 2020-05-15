using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Dictionary<string, Item> _inventory = new Dictionary<string, Item>();

    private int inventorySpace = 9;

    public int takeItem(string[] item, out Item outItem)
    {
        string _item = "";
        outItem = null;
        foreach (string s in item)
        {
            _item += s + " ";
        }
        _item = _item.Trim();

        if (!this.hasItem(_item)) return 0;

        Item i = _inventory[_item];
        if (i == null) return 0;
        outItem = i;
        _inventory.Remove(_item);
        return 1;

    }

    public int addItem(Item item)
    {
        if (item == null) return 0;

        if (!this.hasItem(item.name))
        {
            _inventory.Add(item.name, item);
            return 1;
        }
        return -1;
    }

    public bool hasItem(string item)
    {
        return _inventory.ContainsKey(item);
    }
    public bool hasSpace()
    {
        return (_inventory.Count < inventorySpace);
    }
    public void setSpace(int i)
    {
        this.inventorySpace = i;
    }

    public override string ToString()
    {
        string _temp = "  - ";
        Dictionary<string, Item>.KeyCollection _keys = _inventory.Keys;

        foreach (string s in _keys)
        {
            _temp += s + "\n  - ";
        }
        _temp = _temp.TrimEnd('-', ' ');

        return _temp;
    }

    public Item[] getItems()
    {
        Dictionary<string, Item>.KeyCollection _keys = _inventory.Keys;
        List<Item> items = new List<Item>();

        foreach(string s in _keys)
        {
            items.Add(_inventory[s]);
        }
        
        return items.ToArray();
    }

}
