using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private Inventory inventory = new Inventory();

    private int health = 10;
    private int stamina = 10;

    public bool isAlive = true;

    private Item weapon = null;
    private Item shield = null;
    public bool defending = false;

    public int Health { get => health; set => health = value; }

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

    public int unEquip(string item, ref Item outItem)
    {
        Item i = null;
        if(weapon.name == item)
        {
            i = weapon;
            weapon = null;
        }
        else if(shield.name == item)
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

    public int takeDamage(int dmg,out int outdmg)
    {
        outdmg = dmg;
        if (dmg < 0) return 0;
        if (health - dmg <= 0) return 2;
        health -= dmg;
        return 1;
    }

    public int doDamage(ref Enemy enemy, out int outdmg)
    {
        outdmg = 0;
        if (enemy == null) return -1;
        outdmg = weapon.damage;
        return enemy.takeDamage(weapon.damage);
    }
    public bool canDefend()
    {
        return (shield != null);
    }

    public int defend(int dmg,out int outdmg)
    {
        outdmg = 0;
        if (dmg < shield.damage) return 3;
        return takeDamage(dmg - shield.damage,out outdmg);
    }
}
