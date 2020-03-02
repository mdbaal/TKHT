using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class InputParser : MonoBehaviour
{
    public InputField inputField;
    public GameMaster gameMaster;
    public OutputManager outputManager;

    private string input = "> ";

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

        yield return new WaitUntil(() => input != "" && input !=">_" && input.Length > 3);

        if (checkInput())
        {
            createWords();

            gameMaster.sendInput(words);
        }
        else
        {
            outputManager.outputMessage("You cannot use those characters");
        }

        clearInput();
        StartCoroutine(checkReady());
    }

    private bool checkInput()
    {

        Regex re = new Regex("^[ a-zA-Z>]*$");
        if (!re.IsMatch(input) || input == "> " || input == string.Empty)
        {
            return false;
        }
        return true;
    }

    private void createWords()
    {

       string[]  _words = input.Split(' ');

        for (int i = 1; i < _words.Length; i++) {
            _words[i].Trim();
            string _word = char.ToUpper(_words[i][0]) + _words[i].Substring(1);
            words.Add(_word);
        }
    }

    private void clearInput()
    {
        input = "> ";
        words.Clear();
    }
}
