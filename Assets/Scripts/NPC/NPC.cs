using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC
{
    public int health;

    private Inventory inventory;
    private Item equipedWeapon;

    public virtual void act()
    {

    }

    public int takeDamage(int dmg)
    {
        if(dmg < 0) return 0;
        if (health - dmg <= 0) return 2;
        health -= dmg;
        return 1;
    }

    public int doDamage(ref Player player)
    {
        if (player == null) return -1;

        return player.takeDamage(equipedWeapon.damage);
    }



}
