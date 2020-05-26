using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class InputParser : MonoBehaviour
{
    [SerializeField]
    private InputField inputField;

    
    public GameMaster gameMaster;

    [SerializeField]
    private OutputManager outputManager;

    private string input = "> ";

    private List<string> words = new List<string>();

    private void Start()
    {
        StartCoroutine(checkReady());
        inputField.onEndEdit.AddListener(delegate { if (GameState.readyForPlayerInput) input = inputField.text; else { clearInput(); } });
    }

    IEnumerator checkReady()
    {
        yield return new WaitUntil(() => GameState.readyForPlayerInput == true);

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

        Regex re = new Regex("^[ a-zA-Z>0-9]*$");
        if (!re.IsMatch(input) || input == "> " || input == string.Empty)
        {
            return false;
        }
        return true;
    }

    private void createWords()
    {

       string[]  _words = input.Split(' ');

        if (_words.Length < 2) return;

        foreach(string word in _words)
        {
            if (!word.Contains(">") && word != "" && word != string.Empty) 
            {
                string _word = char.ToUpper(word[0]) + word.Substring(1);
                words.Add(_word);
            }

        }
    }

    private void clearInput()
    {
        input = "> ";
        words.Clear();
    }
}
