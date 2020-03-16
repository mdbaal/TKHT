using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private Inventory inventory = new Inventory();

    private int health = 10;
    private int stamina = 10;
    private int gold = 10;

    private Item left = null;
    private Item right = null;

    public int giveItem(Item item)
    {
        return inventory.addItem(item);
    }

    public int takeItem(string item,ref Item outItem)
    {
       return inventory.takeItem(item,ref outItem);
    }

    public bool hasSpace()
    {
        return inventory.hasSpace();
    }

    public int equip(string item,ref Item outItem)
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
                left = i;
            }
            else
            {
                right = i;
            }
            outItem = i;
            return 1;
        }

        return 0;
    }

    public int unEquip(string item, ref Item outItem)
    {
        Item i = null;
        if(left.name == item)
        {
            i = left;
            left = null;
        }
        else if(right.name == item)
        {
            i = right;
            right = null;
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
