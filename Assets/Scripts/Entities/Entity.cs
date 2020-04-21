using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
    //all values private
    private string _name = "";
    protected Inventory _inventory = new Inventory();
    
    private int _health = 0;
    private int _maxHealth = 0;

    private Weapon _weapon = null;
    private Shield _shield = null;
    private bool _defending = false;

    
    public delegate void DieFunction(); //has to be public
    private DieFunction _dieFunction;

    //all private value getters/setters
    public string name { get => _name; set => _name = value;}
    public int health {
        get { return _health; }
        set {
            _health = value;
            if (_health > maxHealth) _health = maxHealth;
        }
    }
    public int maxHealth { get => _maxHealth; set => _maxHealth = value;}
    public int maxHealthAndHealth
    {
        set { _maxHealth = value; health = value; }
    }
    public Weapon weapon { get => _weapon; set => _weapon = value;}
    public Shield shield { get => _shield; set => _shield = value;}
    public bool isDefending { get => _defending; set => _defending = value;}
    public DieFunction dieFunction { get => _dieFunction; set => _dieFunction = value;}

    public int giveItem(Item item)
    {
        return _inventory.addItem(item);
    }

    public virtual int takeItem(string[] item,  out Item outItem)
    {
        int i = _inventory.takeItem(item, out outItem);

        return i;
    }

    public int takeDamage(int dmg, out int outdmg)
    {
        outdmg = dmg;
        if (dmg < 0) return 0;
        if (health - dmg <= 0) { health -= dmg; return 2; }
        health -= dmg;
        return 1;
    }

    public int doDamage(Entity target, out int outdmg)
    {
        outdmg = 0;
        if (target == null) return -1;
        if (target.isDefending) return target.defend(weapon.damagePoints, out outdmg);
        return target.takeDamage(weapon.damagePoints,out outdmg);
    }

    public bool canDefend()
    {
        return (shield != null);
    }

    public int defend(int dmg, out int outdmg)
    {
        outdmg = 0;
        if (dmg < shield.defencePoints) return 3;
        int r = takeDamage(dmg - shield.defencePoints, out outdmg);
        isDefending = false;
        return r; 
    }

    
    public virtual void die()
    {
        if (dieFunction == null) return;

        this.dieFunction();
    }

    public Item[] getInventoryItems()
    {
        return _inventory.getItems();
    }
}
