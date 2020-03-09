using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Dictionary<string, Item> _inventory = new Dictionary<string, Item>();

    private int inventorySpace = 9;

    public Item takeItem(string item)
    {
        if (!this.hasItem(item)) return null;
        Item i = _inventory[item];
        _inventory.Remove(item);
        return i;
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

    public override string ToString()
    {
        string _temp = "  - ";
        Dictionary<string, Item>.KeyCollection _keys  =  _inventory.Keys;

        foreach (string s in _keys)
        {
          _temp += s + "\n  - ";
        }
       _temp = _temp.TrimEnd('-',' ');

        return _temp;
    }

}
