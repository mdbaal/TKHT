using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    public Player()
    {
        this.maxHealthAndHealth = 20;
        gold = 10;
    }


    //Give quest item
    public int giveItem(string[] item, out QuestItem outItem)
    {
        outItem = null;
        int r = _inventory.addItem(outItem);
        if (r == 0)
        {
            return 0;
        }
        return 1;
    }
    //Take food item
    public int takeItem(string[] item, out Food outItem)
    {
        outItem = null;
        Item _item = null;
        int i = _inventory.takeItem(item, out _item);

        if (_item.GetType() == typeof(Food))
        {
            outItem = (Food)_item;
            return i;
        }
        else
        {
            return 0;
        }
    }
    //Get item with out removing it
    public Item getItem(string[] item)
    {
        return _inventory.getItem(item);
    }

    //Check inventory space
    public bool hasSpace()
    {
        return _inventory.hasSpace();
    }

    //Equip and unequip weapon
    public int equip(string[] item, out Item outItem,out Item unequiped)
    {
        Item i = null;
        outItem = null;
        unequiped = null;
        int returnValue = 0;

        i = this.getItem(item);

        if (i == null) return returnValue;

        if (i.GetType() == typeof(Weapon))
        {
            returnValue = 1;
            if (weapon != null) { this.unEquip(new string[] { weapon.name }, out unequiped); this.giveItem(unequiped); returnValue = 2; }
            weapon = (Weapon)i;
            this.takeItem(item, out i);
            outItem = i;

            return returnValue;
        }
        else if (i.GetType() == typeof(Shield))
        {
            returnValue = 1;
            if (shield != null) { this.unEquip(new string[] { shield.name }, out unequiped); this.giveItem(unequiped); returnValue = 2; }
            shield = (Shield)i;
            this.takeItem(item, out i);
            outItem = i;
            return returnValue;
        }

        return returnValue;

    }

    public int unEquip(string[] item, out Item outItem)
    {

        string _item = "";
        foreach (string s in item)
        {
            _item += s + " ";
        }

        _item = _item.Trim();
        outItem = null;


        if (weapon != null)
        {
            if (weapon.name == _item)
            {
                outItem = weapon;
                weapon = null;
                this.giveItem(outItem);
                return 1;
            }
        }

        if (shield != null)
        {
            if (shield.name == _item)
            {
                outItem = shield;
                shield = null;
                this.giveItem(outItem);
                return 1;
            }
        }

        return 0;
    }

    //Use food item
    public int use(string[] item, out Food outItem)
    {
        outItem = null;
        int r = 0;
        Item i = this.getItem(item);
        if (i == null) return 0;
        if (i.GetType() == typeof(Food))
        {
            r = 1;
        }

        if (r == 0)
        {
            return r;

        }
        else if (health == maxHealth)
        {
            return 2;
        }
        else if (r == 1)
        {
            this.takeItem(item, out outItem);
            this.health += outItem.healingPoints;
            return r;
        }

        return 0;
    }

    public bool hasItem(string item)
    {
        return _inventory.hasItem(item);
    }

}
