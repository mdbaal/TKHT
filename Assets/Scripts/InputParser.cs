using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class InputParser : MonoBehaviour
{
    public InputField inputField;
    public GameMaster gameMaster;

   

    public void checkInput(string input)
    {
        if(!Regex.IsMatch(input, "[a-z0-9 ]", RegexOptions.IgnoreCase))
        {
            Debug.Log("false characters");
            return;
        }

        createWords(input);
    }
    private void createWords(string input)
    {
        List<string> words = new List<string>();
        string[]  _words = input.Split(' ');

        for (int i = 0; i < _words.Length; i++) {
            words.Add(_words[i]);
        }

        gameMaster.sendCommand(words);
    }
}
