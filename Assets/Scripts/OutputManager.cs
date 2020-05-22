using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutputManager : MonoBehaviour
{
    [Header("Output text element")]
    [SerializeField]
    private Text outputField;

    [Header("UI Manager")]
    [SerializeField]
    private UIManager uIManager;


    public void outputMessage(string msg)
    {
        
        outputField.text += "> " + msg;
        outputField.text += "\n";
        uIManager.outputToBottom();
        
    }

    public void outputMessage(string msg,bool bold)
    {
        if (bold)
        {
            outputField.text += "> " + "<b>" + msg + "</b>" ;
            outputField.text += "\n";
            uIManager.outputToBottom();
            return;
        }
        outputField.text += "> " + msg;
        outputField.text += "\n";
        uIManager.outputToBottom();

    }

    public void clear()
    {
        outputField.text = string.Empty;
    }

    public void printHelp(Commands commands)
    {
        string helpString = "These are the commands:";
        foreach(string s in commands.commandsList)
        {
            helpString += "\n- " + s; 
        }

        this.outputMessage(helpString);
    }
}
