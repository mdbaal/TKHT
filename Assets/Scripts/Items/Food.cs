using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Food", menuName = "Food Item", order = 0)]
public class Food : Item
{
    [SerializeField]
    private int _healingPoints = 0;

    public int healingPoints { get => _healingPoints;}
}
