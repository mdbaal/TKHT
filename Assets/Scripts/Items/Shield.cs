using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Shield", menuName = "Shield Item", order = 0)]
public class Shield : Item
{
    [SerializeField]
    private int _defencePoints = 0;

    public int defencePoints { get => _defencePoints; set => _defencePoints = value; }
}
