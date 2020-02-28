using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public Commands commands;
    public OutputManager outputManager;
    
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
            outputManager.outputMessage(action + " " + target);
            doAction();
        }
        else
        {
            outputManager.outputMessage("You cannot do this");
            clearForNew();
        }
    }

    private void doAction()
    {
        switch (action)
        {
            case "Go":
                outputManager.outputMessage("You went to " + target);
                break;
            case "Attack":
                outputManager.outputMessage("Attacking " + target);
                break;
            case "Take":
                outputManager.outputMessage("You took " + target);
                break;
            case "Drop":
                outputManager.outputMessage("You droppped" + target);
                break;
            case "Exit":
            case "Quit":
                outputManager.outputMessage("Goodbye");
                break;
        }

        clearForNew();
    }


    private void clearForNew()
    {
        action = "";
        target = "";
        isReady = true;
    }
}
