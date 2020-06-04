using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trader : NPC
{
    private List<Item> _stock = new List<Item>();

    public Trader(int _newMaxHealth)
    {
        this.maxHealthAndHealth = _newMaxHealth;
    }
    //Get stock
    public List<Item> stock { get => _stock; set => _stock = value; }
    
    //Trader buys from player
    public int buyFromPlayer(Item item)
    {
        if (this.gold - item.worth < 0) return -4;

        this.gold -= item.worth;
        item.worth += Mathf.RoundToInt(item.worth * .2f);
        _stock.Add(item);
        return 1;
    }
    //Trader sells to player
    public int sellToPlayer(Player player, string itemName,out Item itemBought)
    {
        itemBought = null;

        foreach (Item item in _stock)
        {
            if (item.name == itemName)
            {
                if (player.gold - item.worth < 0) return -3;
                itemBought = item;
                player.gold -= itemBought.worth;
                this.gold += itemBought.worth;
                _stock.Remove(item);
                return 1;
            }
        }

        return 0;
    }
    //Get items as string
    public string getListOfStock()
    {
        string list = "Trading \n> Traders gold: " + this.gold.ToString() + "\n > This trader is offering: \n";
        foreach(Item i in stock)
        {
            list += "-  " + i.name +" - G " + i.worth.ToString() + "\n";
        }
        list = list.Trim();

        list += "\nTo stop trading type 'stop'";

        return list;
    }
}
