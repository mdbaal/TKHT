using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    public int health;
    public Item equipedWeapon = null;

    public int takeDamage(int dmg)
    {
        if (dmg < 0) return 0;
        if (health - dmg <= 0) return 2;
        health -= dmg;
        return 1;
    }

    public int doDamage(ref Player player, out int outdmg)
    {
        outdmg = 0;
        if (player == null || equipedWeapon == null) return -1;
        if (player.defending)
        {
            return player.defend(equipedWeapon.damage,out outdmg);
        }
        else
        { 
            return player.takeDamage(equipedWeapon.damage,out outdmg);
        }
    }
}
