using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public Commands commands;

    string action = "";
    string target = "";

    public void sendCommand(List<string> words)
    {
        action = words[0];
        target = words[1];
    }

    private void checkCommand()
    {
        if (commands.checkCommand(action))
        {
            doAction();
            return;
        }
    }

    private void doAction()
    {
        switch(action){
            case "go":

                break;

            
        }
    }

}
