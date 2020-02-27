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

        isReady = false;
        List<string> words = _words;

        if (words.Count == 0 || words == null) return;

        action = words[0];
        target = words[1];



        checkCommand();
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
        Debug.Log("Current action = " + action);
        switch (action)
        {
            case "Go":
                clearForNew();
                break;
            case "Attack":
                clearForNew();
                break;
            case "Defend":

                break;
            case "Take":

                break;
            case "Drop":

                break;
        }
    }


    private void clearForNew()
    {
        action = "";
        target = "";
        isReady = true;
    }
}
