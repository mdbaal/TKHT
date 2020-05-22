using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commands
{
    private string[] _commandsList = new string[]{"Go","Attack","Take","Drop","Look","Exit","Quit","Clear","Help","Equip","Unequip","Use","Save","Load","Trade"};
    private string[] _attackCommandsList = new string[] {"Attack", "Defend", "Flee" };
    private string[] _tradeCommandsList = new string[] {"Buy","Sell","Exit","Quit","Stop"};

    public string[] commandsList { get => _commandsList;}
    public string[] attackCommandsList { get => _attackCommandsList; set => _attackCommandsList = value; }
    public string[] tradeCommandsList { get => _tradeCommandsList; set => _tradeCommandsList = value; }

    public bool checkCommand(string c)
    {
        for(int i = 0; i < _commandsList.Length; i++)
        {
            if (_commandsList[i].Equals(c))
            {
                return true;
            }
        }
        return false;
    }

    public bool checkCombatCommand(string c)
    {
        for (int i = 0; i < _attackCommandsList.Length; i++)
        {
            if (_attackCommandsList[i].Equals(c))
            {
                return true;
            }
        }
        return false;
    }

    public bool checkTradeCommand(string c)
    {
        for (int i = 0; i < _tradeCommandsList.Length; i++)
        {
            if (_tradeCommandsList[i].Equals(c))
            {
                return true;
            }
        }
        return false;
    }
}
