using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Item", order = 0)]
public class Item : ScriptableObject
{
    public new string name;

    public string description;

    public int aantal;

    public int damage;

    public Sprite sprite;
}
