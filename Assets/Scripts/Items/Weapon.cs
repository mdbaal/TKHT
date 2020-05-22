using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon Item", order = 0)]
public class Weapon : Item
{
    [SerializeField]
    private int _damagePoints = 0;

    public int damagePoints { get => _damagePoints; set => _damagePoints = value; }
}
