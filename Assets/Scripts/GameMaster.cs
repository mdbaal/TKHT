using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    private Commands commands = new Commands();
    [Header("Output manager")]
    public OutputManager outputManager;
    [Header("World Locations")]
    public LocationsMap locationsMap;
    [Header("Gamestate")]
    public GameState gameState;
    [Header("UI Manager")]
    public UIManager uIManager;
    [Header("Player")]
    public Player player = new Player();
    
    private string action = "";

    private string[] target;


    private void Start()
    {
        gameState.player = this.player;
    }

    public bool isReady = true;

    public void sendInput(List<string> _words)
    {
        isReady = false;
        List<string> words = _words;

        if (words.Count == 0 || words == null) return;

        action = words[0];
        words.Remove(action);

        target = words.ToArray();

        checkCommand();
    }

    private void checkCommand()
    {
        
        if (commands.checkCommand(action))
        {
            string stringOut = action;

            foreach(string s in target)
            {
                stringOut += " " + s;
            }
            outputManager.outputMessage(stringOut);
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
                locationsMap.move(target);
                break;
            case "Attack":
                outputManager.outputMessage("Attacking " + target[0]);
                break;
            case "Take":
                Item i = locationsMap.GetLocation().takeItem(target[0]);
                player.giveItem(i);
                uIManager.addToPlayerInventory(i.sprite);
                outputManager.outputMessage("You took " + target[0]);
                break;
            case "Drop":
                outputManager.outputMessage("You droppped" + target[0]);
                break;
            case "Look":
                outputManager.outputMessage(locationsMap.getLocationDescription());
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
        target = new string[] { };
        isReady = true;
    }

    private void clearGame()
    {
        action = "";
        target = new string[] { };
    }

    private void quitGame()
    {
        clearGame();
        Application.Quit();
    }
 
}
