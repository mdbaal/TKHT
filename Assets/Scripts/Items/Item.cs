using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Item", order = 0)]
public class Item : ScriptableObject
{
    public new string name;

    public string description;

    public int damage = 0;

    public int amount = 0;

    public int maxAmount = 5;

    public Sprite sprite;


    public int addToAmount(int am)
    {
        if (am + amount > maxAmount) return 0;
        amount += am;

        return 1;
    }
    public int removeFromAmount(int am)
    {
        if (amount - am < 0) return 0;

        amount -= am;
        return 1;
    }
}
