using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutputManager : MonoBehaviour
{
    [Header("Output text element")]
    public Text outputField;

    [Header("UI Manager")]
    public UIManager uIManager;


    public void outputMessage(string msg)
    {
        
        outputField.text += "> " + msg;
        outputField.text += "\n";
        uIManager.toBottom();
        
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
