using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOBJ : MonoBehaviour
{
    private Enemy _enemy;
    [SerializeField]
    private Sprite _sprite;
    [SerializeField]
    private string _name;
    [SerializeField]
    private int _maxHealth;
    [SerializeField]
    private Weapon _weapon;
    [SerializeField]
    private Shield _shield;
    [SerializeField]
    private Item[] _loot;

    private void Awake()
    {
        this._enemy = new Enemy(_maxHealth);
        this._enemy.name = _name;

        this._enemy.weapon = _weapon;
        this._enemy.shield = _shield;

        this._enemy.dieFunction = removeObject;
    }
    public Enemy enemy { get => this._enemy; }
    public Sprite sprite { get => _sprite; set => _sprite = value; }
    public string name { get => _name; set => _name = value; }
    public int maxHealth { get => _maxHealth; set => _maxHealth = value; }
    public Weapon weapon { get => _weapon; set => _weapon = value; }
    public Shield shield { get => _shield; set => _shield = value; }
    public Item[] loot { get => loot; set => loot = value; }

    public void removeObject()
    {
        Destroy(this.gameObject);
    }
}
