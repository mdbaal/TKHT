using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commands
{
    public string[] commandsList = new string[]{"Go","Attack","Take","Drop","Look","Exit","Quit","Clear","Help","Equip","Unequip","Use"};
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

    public bool checkCombatCommand(string c)
    {
        for (int i = 0; i < AttackcommandsList.Length; i++)
        {
            if (AttackcommandsList[i].Equals(c))
            {
                return true;
            }
        }
        return false;
    }
}
