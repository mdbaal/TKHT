﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeManager
{
    private Trader _trader;
    private Player _player;

    public int beginTrade(Player player, Trader trader)
    {
        if (player == null || trader == null) return -1;
        _trader = trader;
        _player = player;

        return 0;
    }

    public void endTrade()
    {
        _player = null;
        _trader = null;
    }

    public int trade(string action, string[] item, out Item _item)
    {
        _item = null;
        if (action == "Buy")
        {
            
            int result = playerIsBuying(item, out _item);
            if (result == 1) return 1;

            return result;
        }
        else if (action == "Sell")
        {
            int result = playerIsSelling(item, out _item);
            if (result == 1) return 2;

            return result;
        }
        else if (action == "Quit" || action == "Exit" || action == "Stop")
        {
            endTrade();
            return 3;
        }

        return -2;
    }




    public int playerIsSelling(string[] item, out Item _item)
    {
        _item = null;
        int result = _player.takeItem(item, out _item);
        if(_item.GetType() == typeof(QuestItem))
        {
            _player.giveItem(_item);
            return -5;
        }
        if (result == 0) { _player.giveItem(_item); return 0; }

        int priceBeforeTrade = _item.worth;
        result = _trader.buyFromPlayer(_item);
        if (result == -4) {
             _player.giveItem(_item);
             return result;
        }
        _player.gold += priceBeforeTrade; 
        return 1;
    }

    public int playerIsBuying(string[] item, out Item _item)
    {
        int result = 0;
        _item = null;
        string newItem = "";
        foreach (string s in item)
        {
            newItem += s;
        }
        newItem = newItem.Trim();

        result = _trader.sellToPlayer(_player, newItem, out _item);

        if (result == 0) return -2;
        if (result == -3) return result;
        if (_item == null) return 0;

        return _player.giveItem(_item);
    }

}