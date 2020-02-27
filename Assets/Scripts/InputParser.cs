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
        yield return new WaitUntil(() => gameMaster.isReady == true);

        StartCoroutine(parseInput());
    }

    IEnumerator parseInput()
    { 

        yield return new WaitUntil(() => input != "");

        if (checkInput())
        {
            createWords();

            gameMaster.sendInput(words);
            clearInput();
            StartCoroutine(checkReady());
        }
        else
        {
            yield return StartCoroutine(parseInput());
        }


    }

    private bool checkInput()
    {

       Regex re = new Regex("^[ a-zA-Z]*$");
        if (!re.IsMatch(input) || input == "")
        {
            return false;
        }
        return true;
    }

    private void createWords()
    {

       string[]  _words = input.Split(' ');

        for (int i = 0; i < _words.Length; i++) {
            _words[i].Trim();
            words.Add(_words[i]);
        }
    }

    private void clearInput()
    {
        input = "";
        words.Clear();
        inputField.text = "";
    }
}
