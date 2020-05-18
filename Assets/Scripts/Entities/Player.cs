using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    public Player()
    {
        this.maxHealthAndHealth = 10;
        gold = 10;
    }


    //Give quest item
    public int giveItem(string[] item, out QuestItem outItem)
    {
        outItem = null;
        int r = _inventory.addItem(outItem);
        if(r == 0)
        {
            return 0;
        }
        return 1;
    }
    //Take food item
    public int takeItem(string[] item, out Food outItem)
    {
        Item _item = null;
        int i = _inventory.takeItem(item, out _item);

        outItem = (Food)_item;

        return i;
    }
    //Check inventory space
    public bool hasSpace()
    {
        return _inventory.hasSpace();
    }

    //Equip and unequip weapon
    public int equip(string[] item, out Item outItem)
    {
        Item i = null;
        outItem = null;
        if (this.takeItem(item, out i) == 0)
        {
            return 0;
        }
        if (i.GetType() == typeof(Weapon))
        {
            weapon = (Weapon) i;
        }
        else if (i.GetType() == typeof(Shield))
        {
            shield = (Shield) i;
        }


        outItem = i;
        return 1;

    }

    public int unEquip(string[] item,out Item outItem)
    {

        string _item = "";
        foreach (string s in item)
        {
            _item += s + " ";
        }
        _item = _item.Trim();
        outItem = null;
        Item i = null;

        if (weapon.name == _item)
        {
            i = weapon;
            weapon = null;
        }else if (shield.name == _item)
        {
            i = shield;
            shield = null;
        }
        else
        {
            return 0;
        }

        this.giveItem(i);
        outItem = i;

        return 1;
    }

    //Use food item
    public int use(string[] item, out Food outItem)
    {
    
        outItem = null;
        if (health == maxHealth) return 2;

        int r = this.takeItem(item, out outItem);
        if (r == 0) return r;
        this.health += outItem.healingPoints;

        return r;

    }
}
