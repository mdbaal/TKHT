using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commands
{
    public string[] commandsList = new string[]{"Go","Attack","Take","Drop","Look","Exit","Quit","Clear","Help","Equip","Unequip","Use",/*"Save","Load",*/"Trade"};
    public string[] attackCommandsList = new string[] {"Attack", "Defend", "Flee" };
    public string[] tradeCommandsList = new string[] {"Buy","Sell","Exit","Quit","Stop"};

    public bool checkCommand(string c)
    {
        for(int i = 0; i < commandsList.Length; i++)
        {
            if (commandsList[i].Equals(c))
            {
                return true;
            }
        }
        return false;
    }

    public bool checkCombatCommand(string c)
    {
        for (int i = 0; i < attackCommandsList.Length; i++)
        {
            if (attackCommandsList[i].Equals(c))
            {
                return true;
            }
        }
        return false;
    }

    public bool checkTradeCommand(string c)
    {
        for (int i = 0; i < tradeCommandsList.Length; i++)
        {
            if (tradeCommandsList[i].Equals(c))
            {
                return true;
            }
        }
        return false;
    }
}
