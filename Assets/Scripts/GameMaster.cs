using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [Header("CombatManager")]
    public CombatManager combatManager;

    private string action = "";

    private string[] target;

    private void Start()
    {

        if (!gameState.finishedTutorial)
        {
            uIManager.startTutorial();
        }
    }


    public int sendInput(List<string> _words)
    {
        gameState.readyForPlayerInput = false;
        List<string> words = _words;

        if (words.Count == 0 || words == null) return 0;

        action = words[0];
        words.Remove(action);
        if(!gameState.inCombat)
        target = words.ToArray();
       
            if (checkCommand() || checkCombatCommand())
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

    private bool checkCombatCommand()
    {
        return commands.checkCombatCommand(action);
    }
    private void checkCombatResult(int r)
    {

        switch (r)
        {
            case -1:

                break;
            case 0:
            case 1:
                gameState.inCombat = false;
                break;
            case 3:
                gameState.inCombat = false;
                StartCoroutine(playerDied());
                break;
        }
    }

    private void doAction()
    {
        int result = 0; //give back result
        Item item = null; //item to use
        string itemName = "";

        if (gameState.inCombat)
        {
            combatManager.nextTurn(action);
            clearForNew();
            return;
        }

        

        switch (action)
        {
            case "Go":
                if (target.Length < 1)
                {
                    outputManager.outputMessage("Go where?");
                    break;
                }

                result = locationsMap.move(target);
                if (result == 1)
                {
                    outputManager.outputMessage("You went to " + locationsMap.getLocationName());
                    gameState.currentLocation = locationsMap.GetLocation();
                }
                else if (result == 0)
                {
                    outputManager.outputMessage("You can't go there");
                }
                else if (result == -1)
                {
                    outputManager.outputMessage("You can't go there from here");
                }
                else if (result == -2)
                {
                    outputManager.outputMessage("You are already at " + gameState.currentLocation.name);
                }
                break;
            case "Attack":
                if (target.Length > 1)
                {
                    outputManager.outputMessage("You can't attack more than one person");
                    break;
                }
                gameState.inCombat = true;
                outputManager.outputMessage("Attacking " + target[0]);
                combatManager.startCombat(locationsMap.GetLocation().getEnemy(target[0]), new CombatManager.CombatCallback(this.checkCombatResult));
                break;
            case "Take":
                if (target.Length == 0)
                {
                    outputManager.outputMessage("You took some air");
                    break;
                }
                if (!gameState.player.hasSpace())
                {
                    outputManager.outputMessage("You don't have enough space");
                }
                //get the item
                result = locationsMap.GetLocation().takeItem(target,  out item);

                if (result == 0)
                {
                    outputManager.outputMessage("This item doesn't exist");
                }
                else if (result == 1)
                {
                   result = gameState.player.giveItem(item);
                    uIManager.addToPlayerInventory(item);
                    if (item.GetType() == typeof(QuestItem))
                    {
                        int i =  gameState.addToQuestItems((QuestItem)item);
                        uIManager.UpdateObjectiveText(i);
                        outputManager.outputMessage("You took " + item.name + " It's one of the quest items!");
                        if (gameState.allQuestItemsCollected)
                        {
                            StartCoroutine(PlayerWon());
                        }
                    }
                    else{
                        outputManager.outputMessage("You took " + item.name);
                    }
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
                result = gameState.player.takeItem(target,  out item);

                if (result == 0)
                {
                    outputManager.outputMessage("This item doesn't exist");
                }
                else if (result == 1)
                {
                    locationsMap.GetLocation().dropItem(item);
                    uIManager.removeFromPlayerInventory(item);
                    outputManager.outputMessage("You dropped " + item.name);
                }
                else if(result == -1)
                {
                    outputManager.outputMessage("You can't drop quest items");
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
            case "Equip":

                result = gameState.player.equip(target,out item);
                
                if (result == 0)
                {
                    outputManager.outputMessage("You can't equip that item");
                }
                else if (result == 1)
                {
                    uIManager.addToEquiped(item);
                    outputManager.outputMessage("You equiped " + item.name);
                }
                else if (result == -1)
                {
                    outputManager.outputMessage("Already have something equiped there");
                }
                break;
            case "Unequip":
                result = gameState.player.unEquip(target,  out item);

                if (result == 0)
                {
                    outputManager.outputMessage("You don't have that item equiped");
                }
                else if (result == 1)
                {
                    uIManager.removeFromEquiped(item);
                    outputManager.outputMessage("You unequiped " + item.name);
                }
                break;
            case "Use":
                Food _item;
                result = gameState.player.use(target,out _item);

                if (result == -1 || result == 0)
                {
                    outputManager.outputMessage("That isn't a usable item");
                }else if(result == 1)
                {
                    uIManager.removeFromPlayerInventory(_item);
                    uIManager.updatePlayerHealth( gameState.player);
                    outputManager.outputMessage("You used " + _item.name);
                }else if(result == 2)
                {
                    outputManager.outputMessage("You are already at full health");
                }
                break;
        }

        clearForNew();
    }
    IEnumerator PlayerWon()
    {
        uIManager.showEndscreen(1);
        yield return new WaitForSeconds(1f);
        StartCoroutine(restartGameOnEnter());
        StartCoroutine(quitGameOnEscape());
    }

    IEnumerator playerDied()
    {
        uIManager.showEndscreen(0);
        yield return new WaitForSecondsRealtime(1f);
        StartCoroutine(restartGameOnEnter());
        StartCoroutine(quitGameOnEscape());

    }
    IEnumerator restartGameOnEnter()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        uIManager.closeEndScreen();
        StopCoroutine(quitGameOnEscape());
        restartGame();
    }
    IEnumerator quitGameOnEscape()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Escape));
        StopCoroutine(restartGameOnEnter());
        uIManager.closeEndScreen();
        quitGame();
    }


    private void clearForNew()
    {
        action = "";
        target = new string[] { };
        gameState.readyForPlayerInput = true;
    }

    private void restartGame()
    {
        action = "";
        target = new string[] { };
        SceneManager.LoadSceneAsync("Main");
        gameState.restart();
    }

    private void quitGame()
    {
        restartGame();
        Application.Quit();
    }

}
