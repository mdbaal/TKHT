using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Dictionary<string, Item> _inventory = new Dictionary<string, Item>();

    private int inventorySpace = 9;

    //Take item ou of inventory
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
    //Add item to inventory
    public int addItem(Item item)
    {
        if (item == null) return 0;
        if (!this.hasSpace()) return -2;

        if (!this.hasItem(item.name))
        {
            _inventory.Add(item.name, item);
            return 1;
        }
        return -1;
    }
    //Get item with out removing it
    public Item getItem(string[] _item)
    {
        string item = "";

        foreach (string s in _item)
        {
            item += s + " ";
        }
        item = item.Trim();
        if (!_inventory.ContainsKey(item)) return null;

        return _inventory[item];
    }

    //Is the item in the inventory
    public bool hasItem(string item)
    {
        return _inventory.ContainsKey(item);
    }
    //Is there enough space
    public bool hasSpace()
    {
        return (_inventory.Count < inventorySpace);
    }
    //Set or get the size of the inventory
    public int space
    {
        get => this.inventorySpace; set => this.inventorySpace = value;
    }
    
    //clear Inventory
    public void clearInventory()
    {
        _inventory.Clear();
        _inventory = new Dictionary<string, Item>();
    }

    //Output list of items in inventory
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

    //Get all items in inventory as array
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
