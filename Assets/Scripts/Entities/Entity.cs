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

    private Item _weapon = null;
    private Item _shield = null;
    private bool _defending = false;

    
    public delegate void DieFunction(); //has to be public
    private DieFunction _dieFunction;

    //all private value getters/setters
    public string name { get => _name; set => _name = value;}
    public int health { get => _health; set => _health = value;}
    public int maxHealth { get => _maxHealth; set => _maxHealth = value;}
    public int maxHealthAndHealth
    {
        set { _maxHealth = value; health = value; }
    }
    public Item weapon { get => _weapon; set => _weapon = value;}
    public Item shield { get => _shield; set => _shield = value;}
    public bool defending { get => _defending; set => _defending = value;}
    public DieFunction dieFunction { get => _dieFunction; set => _dieFunction = value;}

    public int giveItem(Item item)
    {
        return _inventory.addItem(item);
    }

    public virtual int takeItem(string[] item, ref Item outItem)
    {
        int i = _inventory.takeItem(item, ref outItem);

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

    public int doDamage(Entity entity, out int outdmg)
    {
        outdmg = 0;
        if (entity == null) return -1;
        
        return entity.takeDamage(_weapon.damage,out outdmg);
    }

    public bool canDefend()
    {
        return (_shield != null);
    }

    public int defend(int dmg, out int outdmg)
    {
        outdmg = 0;
        if (dmg < _shield.damage) return 3;
        return takeDamage(dmg - _shield.damage, out outdmg);
    }

    public int use(string[] item, out Item outItem)
    {
        outItem = null;
        Item _item = null;
        int result = this._inventory.takeItem(item, ref _item);
        if (result == 0) return result;
        if (_item.isConsumable && _item != null)
        {
            if (this.health == maxHealth) { this._inventory.addItem(_item); return 2; }
            this.health = this.health + _item.healing;

            if (health > maxHealth) health = maxHealth;
            outItem = _item;
            return 1;
        }
        
        return -1;

    }
    public virtual void die()
    {
        if (dieFunction == null) return;

        this.dieFunction();
    }

}
