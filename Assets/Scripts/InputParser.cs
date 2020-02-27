using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class InputParser : MonoBehaviour
{
    public InputField inputField;
    public GameMaster gameMaster;

    private string input = "";

    private List<string> words = new List<string>();

    private void Start()
    {
        StartCoroutine(checkReady());
        inputField.onEndEdit.AddListener(delegate { input = inputField.text; });
    }

    IEnumerator checkReady()
    {
        Debug.Log("Wait for ready");
        yield return new WaitUntil(() => gameMaster.isReady == true);
        Debug.Log("Ready");
        StartCoroutine(parseInput());
    }

    IEnumerator parseInput()
    {
        Debug.Log("Waiting for input");
        input = "";
        inputField.text = "";

        yield return new WaitUntil(() => input != "");
        Debug.Log("Parsing input");
        inputField.text = "";

        if (checkInput())
        {
            createWords();
            Debug.Log("Sending parsed input");
            gameMaster.sendInput(words);
            StartCoroutine(checkReady());
        }
        else
        {
            yield return StartCoroutine(parseInput());
        }


    }

    private bool checkInput()
    {
        Debug.Log("Checking input");
       Regex re = new Regex("^[ a-zA-Z]*$");
        if (!re.IsMatch(input) || input == "")
        {
            Debug.Log("Bad input");
            return false;
        }
        Debug.Log("Good input");
        return true;
    }

    private void createWords()
    {
        Debug.Log("Creating words");
       string[]  _words = input.Split(' ');

        for (int i = 0; i < _words.Length; i++) {
            _words[i].Trim();
            words.Add(_words[i]);
        }
    }
}
