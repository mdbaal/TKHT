using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commands : ScriptableObject
{
    public List<string> commands;

    public bool checkCommand(string c)
    {
        for(int i = 0; i < commands.Count; i++)
        {
            if (commands.Contains(c)) return true;
        }

        return false;
    }
}
