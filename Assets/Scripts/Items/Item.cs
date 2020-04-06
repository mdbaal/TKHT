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

    public int healing = 0;

    public Sprite sprite = null;

    public bool equipable = false;

    public bool equipLeft = true;

    public bool isQuestItem = false;

    public bool isConsumable = false;
}
