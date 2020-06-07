using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private Player player = null;
    private Enemy enemy = null;

    private bool playerTurn = true;
    [Header("Time to wait in seconds")]
    [SerializeField]
    private int timeToWait = 0;
    [SerializeField]
    private OutputManager outputManager;
    [SerializeField]
    private UIManager uIManager;

    public delegate void CombatCallback(int r);

    public CombatCallback combatCallback;

    private void Start()
    {
        StartCoroutine(setPlayer());
    }

    IEnumerator setPlayer()
    {
        yield return new WaitUntil(() => GameState.player != null);
        player = GameState.player;
    }
    //Start combat, asign enemt and call back and call attack action
    public int startCombat(Enemy enemy, CombatCallback c)
    {
        if (this.combatCallback != c) this.combatCallback = c;

        if(GameState.player.weapon == null)
        {
            outputManager.outputMessage("You don't have a weapon equiped");
            return -1;
        }

        if (enemy == null)
        {
            outputManager.outputMessage("That isn't an enemy");
            return -1;
        }

        this.enemy = enemy;
        return 1;
    }
    //Next turn in combat based on action and if it is the players turn
    public void nextTurn(string action)
    {
        int result = 0;
        int dmgDone = 0;
        if (playerTurn)
        {
            
            switch (action)
            {
                case "Attack":
                    result = player.doDamage(this.enemy, out dmgDone);
                    if (result == 0)
                    {
                        outputManager.outputMessage("You didn't do any damage");
                        playerTurn = false;
                        combatCallback(-1);
                        StartCoroutine(waitBeforeComputerAction());
                        break;
                    }
                    else if (result == 1)
                    {
                        outputManager.outputMessage("You hit them with " + dmgDone + " damage");
                        playerTurn = false;
                        StartCoroutine(waitBeforeComputerAction());
                        combatCallback(-1);
                        break;
                    }
                    else if (result == 2)
                    {
                        outputManager.outputMessage("You hit with a finishing blow");
                        enemy.die();
                        endCombat();
                        combatCallback(0);
                        break;
                    }
                    else if(result == -2)
                    {
                        outputManager.outputMessage("You don't have a weapon equipped");
                        endCombat();
                        combatCallback(0);
                        break;
                    }
                    break;
                case "Defend":
                    if (!player.canDefend())
                    {
                        outputManager.outputMessage("You don't have a shield equipped");
                        playerTurn = false;
                        combatCallback(-1);
                        StartCoroutine(waitBeforeComputerAction());
                        break;
                    }
                    outputManager.outputMessage("You defend");
                    player.isDefending = true;
                    playerTurn = false;
                    combatCallback(-1);
                    StartCoroutine(waitBeforeComputerAction());
                    break;
                case "Flee":
                    outputManager.outputMessage("You flee from the battle");
                    enemy.health = enemy.maxHealth;
                    endCombat();
                    combatCallback(2);
                    break;
                case "Help":
                    outputManager.printHelp(new Commands());
                    break;
            }
        }
        else
        {
            outputManager.outputMessage(enemy.name + " attacks");
            result = enemy.doDamage(player, out dmgDone);

            if (result == -1 || result == 0)
            {
                outputManager.outputMessage("You didn't take any damage");
                playerTurn = true;
                combatCallback(-1);
            } else if (result == 1)
            {
                outputManager.outputMessage("You took " + dmgDone + " damage");
                uIManager.updatePlayerHealth( player);
                playerTurn = true;
                combatCallback(-1);
            } else if (result == 2) {
                outputManager.outputMessage("You have been killed");
                uIManager.updatePlayerHealth( player);
                endCombat();
                combatCallback(3);
            }else if (result == 3)
            {
                outputManager.outputMessage("You fully blocked the attack");
                playerTurn = true;
                combatCallback(-1);
                
            }
        }
    }
    public void endCombat()
    {
        this.enemy = null;
    }

    IEnumerator waitBeforeComputerAction()
    {
        yield return new WaitForSeconds(timeToWait);
        nextTurn("");
    }
}
