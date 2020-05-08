using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraderOBJ : MonoBehaviour
{
    private Trader _trader;

    public Sprite _sprite;
    public string _name;
    public int _maxHealth;
    public Weapon _weapon;
    public int _gold;

    public List<Item> stock;



    private void Awake()
    {
        this._trader = new Trader(_maxHealth);
        this._trader.name = _name;

        this._trader.weapon = _weapon;

        this._trader.dieFunction = removeObject;

        this._trader.gold = _gold;

        this._trader.stock = this.stock;
    }

    public Trader trader { get => this._trader; }

    public void removeObject()
    {
        Destroy(this.gameObject);
    }
}