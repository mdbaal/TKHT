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

    public int sendInput(List<string> _words)
    {
        isReady = false;
        List<string> words = _words;

        if (words.Count == 0 || words == null) return 0;

        action = words[0];
        words.Remove(action);

        target = words.ToArray();

        checkCommand();
        return 1;
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
        int result = 0;
        Item item = null;
        switch (action)
        {
            case "Go":
                result = locationsMap.move(target);
                if (result == 1)
                {
                    outputManager.outputMessage("You went to " + locationsMap.getLocationName());
                }
                else if(result == 0)
                {
                    outputManager.outputMessage("Can't go there");
                }
                else if(result == -1)
                {
                    outputManager.outputMessage("Can't go there from here");
                }
                break;
            case "Attack":
                outputManager.outputMessage("Attacking " + target[0]);
                break;
            case "Take":
                locationsMap.GetLocation().takeItem(target[0],out item);
                result = player.giveItem(item);
                if (result == 1)
                {
                    uIManager.addToPlayerInventory(item.sprite);
                    outputManager.outputMessage("You took " + target[0]);
                }
                else if (result == 0)
                {
                    outputManager.outputMessage("This item doesn't exist");
                }
                else
                {
                    outputManager.outputMessage("You already have this item");
                }
                break;
            case "Drop":
                item = player.takeItem(target[0]); 
                result = locationsMap.GetLocation().dropItem(item);
                if (result == 1)
                {
                    uIManager.removeFromPlayerInventory(item.sprite);
                    outputManager.outputMessage("You droppped" + target[0]);
                }
                else if (result == 0)
                {
                    outputManager.outputMessage("You don't have this item");
                }
                else if (result == -1)
                {
                    outputManager.outputMessage("You can't drop this here");
                }
                
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
