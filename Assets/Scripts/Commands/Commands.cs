using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Commands", menuName = "Commands object", order = 0)]
public class Commands : ScriptableObject
{
    public string[] commands = new string[]{"Go","Attack","Defend","Take","Drop"};
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
