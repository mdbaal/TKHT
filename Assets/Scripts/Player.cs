using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private Inventory inventory = new Inventory();

    private int health = 10;
    private int stamina = 10;
    private int gold = 10;

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
}
