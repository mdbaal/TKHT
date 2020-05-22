using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraderOBJ : MonoBehaviour
{
    private Trader _trader;
    [SerializeField]
    private Sprite _sprite;
    [SerializeField]
    private string _name;
    [SerializeField]
    private int _maxHealth;
    [SerializeField]
    private Weapon _weapon;
    [SerializeField]
    private int _gold;
    [SerializeField]
    private List<Item> _stock;



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
    public Sprite sprite { get => _sprite; set => _sprite = value; }
    public string name { get => _name; set => _name = value; }
    public int maxHealth { get => _maxHealth; set => _maxHealth = value; }
    public Weapon weapon { get => _weapon; set => _weapon = value; }
    public int gold { get => _gold; set => _gold = value; }
    public List<Item> stock { get => _stock; }

    public void removeObject()
    {
        Destroy(this.gameObject);
    }
}