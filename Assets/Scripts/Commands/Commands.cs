using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commands
{
    public string[] commandsList = new string[]{"Go","Attack","Defend","Take","Drop","Look","Exit","Quit","Clear","Help","Equip","Unequip"};
    public string[] AttackcommandsList = new string[] {"Attack", "Defend", "Flee" };

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
}
