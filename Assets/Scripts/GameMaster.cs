using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private Commands commands = new Commands();
    [Header("Output manager")]
    [SerializeField]
    private OutputManager outputManager;
    [Header("World Locations")]
    [SerializeField]
    private LocationsMap locationsMap;

    [Header("UI Manager")]
    [SerializeField]
    private UIManager uIManager;
    [Header("CombatManager")]
    [SerializeField]
    private CombatManager combatManager;
    [Header("AudioManager")]
    [SerializeField]
    private AudioManager audioManager; 

    private TradeManager tradeManager = new TradeManager();

    private string action = "";

    private string[] target;
    
    private void Start()
    {
        StartCoroutine(setupGameOnStart());
    }

     IEnumerator setupGameOnStart()
    {
        locationsMap.makeLocations();

        yield return new WaitUntil(() => locationsMap.allLocationsMade);

        if (!this.loadGame())
        {
            if (!GameState.finishedTutorial)
            {
                uIManager.startTutorial();
            }
            GameState.currentLocation = locationsMap.getLocation();
        }
        
        if (GameState.allQuestItemsCollected)
        {
            yield return StartCoroutine(playerWon());
        }

        if (!GameState.readyForPlayerInput) GameState.readyForPlayerInput = true;

        if (GameState.isTrading) GameState.isTrading = false;

        GameState.readyForPlayerInput = true;

        audioManager.changeSong();
    }

    //Called form input parser, receive input
    public int sendInput(List<string> _words)
    {
        GameState.readyForPlayerInput = false;
        List<string> words = _words;

        if (words.Count == 0 || words == null) return 0;

        action = words[0];
        words.Remove(action);

        if (!GameState.inCombat)
            target = words.ToArray();

        if (checkCommand() || checkCombatCommand() || checkTradeCommand())
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
    //Multiple checks to keep it all seperate and clear
    private bool checkCommand()
    {
        return commands.checkCommand(action);
    }

    private bool checkCombatCommand()
    {
        return commands.checkCombatCommand(action);
    }

    private bool checkTradeCommand()
    {
        return commands.checkTradeCommand(action);
    }


    //Call back for combat, to check for action after combat
    private void checkCombatResult(int r)
    {

        switch (r)
        {
            case -1:

                break;
            case 0:
            case 1:
            case 2:
                GameState.inCombat = false;
                uIManager.toggleCombatEdge();
                GameState.currentLocation.enemyDied();
                GameState.currentLocation.checkAllDead();
                audioManager.changeSong();
                GameState.player.gold += Random.Range(0,6);
                break;
            case 3:
                GameState.inCombat = false;
                StartCoroutine(playerDied());
                break;
        }
    }

    //Decide based on action what to do
    private void doAction()
    {
        int result = 0; //give back result
        Item item = null; //item to use

        if (GameState.inCombat)
        {
            combatManager.nextTurn(action);
            clearForNew();
            return;
        }

        if (GameState.isTrading)
        {
            outputManager.outputMessage("---------------------------------");
            result = tradeManager.trade(action, target, out item);
            if (result == 0) outputManager.outputMessage("That item doesn't exist", true);
            if (result == -1) outputManager.outputMessage("You already have that item in your inventory", true);
            if (result == -2) outputManager.outputMessage("That isn't a trade command", true);
            if (result == -3) outputManager.outputMessage("You don't have enough gold", true);
            if (result == -4) outputManager.outputMessage("The trader doesn't have enough gold", true);
            if (result == -5) outputManager.outputMessage("You can't sell a quest item", true);
            if (result == 1)
            {
                outputManager.outputMessage("You have bought " + item.name + " for " + item.worth + " gold", true);
                uIManager.addToPlayerInventory(item); uIManager.updateGold();

                if (item.GetType() == typeof(QuestItem))
                {
                    GameState.addToQuestItems((QuestItem)item);
                    uIManager.UpdateObjectiveText((QuestItem)item);
                    outputManager.outputMessage("It's one of the quest items!");

                    if (GameState.allQuestItemsCollected)
                    {
                        StartCoroutine(playerWon());
                    }
                }
            }
            if (result == 2) { outputManager.outputMessage("You have sold " + item.name + " for " + item.worth + " gold"); uIManager.removeFromPlayerInventory(item); uIManager.updateGold(); }
            if (result == 3) { GameState.isTrading = false; outputManager.outputMessage("You stopped trading"); }

            if (result != 3) outputManager.outputMessage(locationsMap.getLocation().getTrader().getListOfStock());
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
                    GameState.currentLocation = locationsMap.getLocation();
                    uIManager.UpdateMinimap(GameState.currentLocation.name);
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
                    outputManager.outputMessage("You are already at " + GameState.currentLocation.name);
                }
                else if (result == -3)
                {
                    outputManager.outputMessage("You can't travel when there are enemies near");
                }
                break;
            case "Attack":
                string targetName = "";
                foreach (string s in target)
                {
                    targetName += s + " ";
                }

                targetName = targetName.Trim();

                result = combatManager.startCombat(locationsMap.getLocation().getEnemy(targetName), new CombatManager.CombatCallback(this.checkCombatResult));

                if (result == 1)
                { 
                    GameState.inCombat = true;
                    audioManager.changeSong();
                    uIManager.toggleCombatEdge();
                    
                }
                break;
            case "Take":
                if (target.Length == 0)
                {
                    outputManager.outputMessage("You took some air");
                    break;
                }

                //get the item
                item = GameState.currentLocation.getItem(target);

                result = GameState.player.giveItem(item);

                if (result == 0)
                {
                    outputManager.outputMessage("This item doesn't exist");
                    break;
                }
                else if (result == 1)
                {
                    uIManager.addToPlayerInventory(item);

                    if (item.GetType() == typeof(QuestItem))
                    {
                        GameState.addToQuestItems((QuestItem)item);
                        uIManager.UpdateObjectiveText((QuestItem)item);
                        outputManager.outputMessage("You took " + item.name + " It's one of the quest items!");
                        if (GameState.allQuestItemsCollected)
                        {
                            StartCoroutine(playerWon());
                        }
                    }
                    else
                    {
                        outputManager.outputMessage("You took " + item.name);
                    }
                    GameState.currentLocation.takeItem(target, out item);
                    GameState.player.giveItem(item);
                    audioManager.playPickupDropSound();
                }
                else if (result == -2)
                {
                    outputManager.outputMessage("You don't have enough space");
                    break;
                }
                else if (result == -1)
                {
                    outputManager.outputMessage("You already have that item");
                }

                break;
            case "Drop":
                if (target.Length == 0)
                {
                    outputManager.outputMessage("You dropped some air");
                    break;
                }

                //drop the item
                item = GameState.player.getItem(target);

                result = GameState.currentLocation.dropItem(item);

                if (result == 0)
                {
                    outputManager.outputMessage("This item doesn't exist");
                }
                else if (result == 1)
                {
                    GameState.player.takeItem(target,out item);
                    uIManager.removeFromPlayerInventory(item);
                    outputManager.outputMessage("You dropped " + item.name);
                    audioManager.playPickupDropSound();
                }
                else if (result == -1)
                {
                    outputManager.outputMessage("You can't drop quest items");
                }
                else if(result == -2)
                {
                    outputManager.outputMessage("There is no place to put this");
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
                Item unequiped = null;
                result = GameState.player.equip(target, out item,out unequiped);

                if (result == 0)
                {
                    outputManager.outputMessage("You can't equip that item");
                }
                else if (result == 1)
                {
                    uIManager.addToEquiped(item);
                    outputManager.outputMessage("You equipped " + item.name);
                    audioManager.playPickupDropSound();
                }else if(result == 2)
                {
                    uIManager.addToEquiped(item);
                    if (unequiped != null) uIManager.addToPlayerInventory(unequiped);
                    outputManager.outputMessage("You equipped " + item.name);
                    audioManager.playPickupDropSound();

                }
                else if (result == -1)
                {
                    outputManager.outputMessage("Already have something equipped there");
                }
                break;
            case "Unequip":
                result = GameState.player.unEquip(target, out item);

                if (result == 0)
                {
                    outputManager.outputMessage("You don't have that item equipped");
                }
                else if (result == 1)
                {
                    uIManager.removeFromEquiped(item);
                    outputManager.outputMessage("You unequiped " + item.name);
                    audioManager.playPickupDropSound();
                }
                break;
            case "Use":
                Food _item;
                result = GameState.player.use(target, out _item);

                if (result == -1 || result == 0)
                {
                    outputManager.outputMessage("That isn't a usable item");
                }
                else if (result == 1)
                {
                    uIManager.removeFromPlayerInventory(_item);
                    uIManager.updatePlayerHealth(GameState.player);
                    outputManager.outputMessage("You used " + _item.name);
                }
                else if (result == 2)
                {
                    outputManager.outputMessage("You are already at full health");
                }
                break;
            case "Save":
                this.saveGame();
                outputManager.outputMessage("Game saved");
                break;
            case "Load":
                this.loadGame();
                outputManager.clear();
                outputManager.outputMessage("Game loaded");
                break;
            case "Trade":
                Trader trader = locationsMap.getLocation().getTrader();
                if (trader != null)
                {

                    tradeManager.beginTrade(GameState.player, trader);
                    GameState.isTrading = true;
                    outputManager.outputMessage(locationsMap.getLocation().getTrader().getListOfStock());
                    outputManager.outputMessage("To stop trading type stop,quit or exit\n > Do you want to Buy or Sell");
                    break;
                }
                else
                {
                    outputManager.outputMessage("There is no trader here");
                    break;
                }
            case "Restart":
                restartGame();
                break;
        }

        clearForNew();
    }

    IEnumerator playerWon()
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

    public void saveGame()
    {
        SaveLoadManager.save(this.locationsMap);
    }

    public bool loadGame()
    {
        return SaveLoadManager.load(this.locationsMap, this.uIManager);
    }



    private void clearForNew()
    {
        action = "";
        target = new string[] { };
        GameState.readyForPlayerInput = true;
    }

    private void restartGame()
    {
        action = "";
        target = new string[] { };
        SaveLoadManager.deleteSaveGame();
        SceneManager.LoadSceneAsync("Main");
        GameState.restart();
    }

    private void quitGame()
    {
        restartGame();
        Application.Quit();
    }
}
