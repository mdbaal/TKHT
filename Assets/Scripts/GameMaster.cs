using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public Commands commands;

    private string action = "";
    private string target = "";


    public bool isReady = true;

    public void sendInput(List<string> _words)
    {
        Debug.Log("Received parsed input");
        isReady = false;
        List<string> words = _words;

        if (words.Count == 0 || words == null) return;

        action = words[0];
        target = words[1];

        Debug.Log("action: " + action + " | target: " + target);

        checkCommand();
    }

    private void checkCommand()
    {
        Debug.Log("Checking command");
        if (commands.checkCommand(action))
        {
            doAction();
            return;
        }
    }

    private void doAction()
    {
        switch(action){
            case "Go":
                Debug.Log("Do action Go");
                isReady = true;
                break;
            case "Attack":

                break;
            case "Defend":

                break;
            case "Take":

                break;
            case "Drop":

                break;
        }
    }

}
