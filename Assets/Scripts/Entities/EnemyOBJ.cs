using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOBJ : MonoBehaviour
{
    private Enemy _enemy;

    public Sprite _sprite;
    public string _name;
    public int _maxHealth;
    public Weapon _weapon;
    public Shield _shield;

    public Item[] loot;

    private void Awake()
    {
        this._enemy = new Enemy(_maxHealth);
        this._enemy.name = _name;

        this._enemy.weapon = _weapon;
        this._enemy.shield = _shield;

        this._enemy.dieFunction = removeObject;
    }
    public Enemy enemy { get => this._enemy; }

    public void removeObject()
    {
        Destroy(this.gameObject);
    }
}
