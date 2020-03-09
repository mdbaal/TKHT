using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private Inventory inventory = new Inventory();

    private int health = 10;
    private int stamina = 10;
    private int gold = 10;

    public void giveItem(Item item)
    {
        inventory.addItem(item);
    }

    public void takeItem(string item)
    {
        inventory.getItem(item);
    }
}
