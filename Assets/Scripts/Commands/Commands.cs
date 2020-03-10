using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commands
{
    public string[] commands = new string[]{"Go","Attack","Defend","Take","Drop","Look","Exit","Quit","Clear"};
    public string[] Attackcommands = new string[] {"Attack", "Defend", "Flee" };

    public bool checkCommand(string c)
    {
        for(int i = 0; i < commands.Length; i++)
        {
            if (commands[i].Equals(c))
            {
                return true;
            }
        }
        return false;
    }
}
