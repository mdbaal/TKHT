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

        if (checkCommand())
        {
            string stringOut = action;

            foreach (string s in target)
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
            return -1;
        }
        return 1;
    }

    private bool checkCommand()
    {
        return commands.checkCommand(action);
    }

    private void doAction()
    {
        int result = 0; //give back result
        Item item = null; //item to use

        switch (action)
        {
            case "Go":
                if(target.Length < 1)
                {
                    outputManager.outputMessage("Go where?");
                    break;
                }

                result = locationsMap.move(target);
                if (result == 1)
                {
                    outputManager.outputMessage("You went to " + locationsMap.getLocationName());
                }
                else if(result == 0)
                {
                    outputManager.outputMessage("You can't go there");
                }
                else if(result == -1)
                {
                    outputManager.outputMessage("You can't go there from here");
                }else if(result == -2)
                {
                    outputManager.outputMessage("You are already there");
                }
                break;
            case "Attack":
                if (target.Length > 1)
                {
                    outputManager.outputMessage("You can't attack more than one person");
                }
                outputManager.outputMessage("Attacking " + target[0]);
                break;
            case "Take":
                if (target.Length == 0)
                {
                    outputManager.outputMessage("You took some air");
                    break;
                }
                if (!player.hasSpace())
                {
                    outputManager.outputMessage("You don't have enough space");
                }
                //get the item
                result = locationsMap.GetLocation().takeItem(target[0], ref item);

                if (result == 0)   
                {
                    outputManager.outputMessage("This item doesn't exist");
                }
                else if (result == 1)
                {
                    player.giveItem(item);
                    uIManager.addToPlayerInventory(item);
                    outputManager.outputMessage("You took " + target[0]);
                }
                
                break;
            case "Drop":
                if (target.Length == 0)
                {
                    outputManager.outputMessage("You dropped some air");
                    break;
                }
                if (!locationsMap.GetLocation().hasSpace())
                {
                    outputManager.outputMessage("There is no place to put this");
                }
                
                //drop the item
                result = player.takeItem(target[0],ref item);

                if (result == 0)
                {
                    outputManager.outputMessage("This item doesn't exist");
                }
                else if (result == 1)
                {
                    locationsMap.GetLocation().dropItem(item);
                    uIManager.removeFromPlayerInventory(item);
                    outputManager.outputMessage("You dropped " + target[0]);
                }

                break;
            case "Look":
                outputManager.outputMessage(locationsMap.getLocationDescription());
                break;
            case "Exit":
            case "Quit":
                outputManager.outputMessage("Goodbye");
                quitGame();
                break;
            case "Clear":
                outputManager.clear();
                break;
            case "Help":
                outputManager.printHelp(commands);
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
