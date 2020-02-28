using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutputManager : MonoBehaviour
{

    public Text outputField;


    public void outputMessage(string msg)
    {
        outputField.text += "> " + msg;
        outputField.text += "\n";
    }
}
