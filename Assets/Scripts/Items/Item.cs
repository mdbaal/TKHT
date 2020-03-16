using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Item", order = 0)]
public class Item : ScriptableObject
{
    public new string name;
    [TextArea()]
    public string description;

    public int damage = 0;

    public Sprite sprite;

    public bool equipable = false;

    public bool equipLeft = true;
}
