using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Dictionary<string, Item> _inventory = new Dictionary<string, Item>();

    public Item getItem(string item)
    {
        return _inventory[item];
    }

    public void addItem(Item item)
    {
        _inventory.Add(item.name, item);
    }

    public bool hasItem(string item)
    {
        return _inventory.ContainsKey(item);
    }

    public override string ToString()
    {
        string _temp = "";
        Dictionary<string, Item>.KeyCollection _keys  =  _inventory.Keys;

        foreach (string s in _keys)
        {
            _temp += s + ",";
        }
        _temp.TrimEnd(',');

        return _temp;
    }

}
