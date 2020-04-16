using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    
    public Player()
    {
        this.maxHealthAndHealth = 10;
    }

    public override int takeItem(string[] item, ref Item outItem)
    {
        int i = _inventory.takeItem(item, ref outItem);

        if (outItem.isQuestItem)
        {
            _inventory.addItem(outItem);
            return -1;
        }

        return i;
    }

    public bool hasSpace()
    {
        return _inventory.hasSpace();
    }

    public int equip(string[] item, ref Item outItem)
    {
        Item i = null;

        if (this.takeItem(item, ref i) == 0)
        {
            return 0;
        }

        if (i.equipable)
        {
            if (i.equipLeft)
            {
                weapon = i;
            }
            else
            {
                shield = i;
            }
            outItem = i;
            return 1;
        }

        return 0;
    }

    public int unEquip(string[] item, ref Item outItem)
    {

        string _item = "";
        foreach (string s in item)
        {
            _item += s + " ";
        }
        _item = _item.Trim();

        Item i = null;
        if (weapon.name == _item)
        {
            i = weapon;
            weapon = null;
        }
        else if (shield.name == _item)
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
}
