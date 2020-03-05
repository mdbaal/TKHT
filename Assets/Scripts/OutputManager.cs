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
}
