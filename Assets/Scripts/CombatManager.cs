using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("Game State")]
    public GameState gameState;

    private Player player = null;
    private Enemy enemy = null;

    private bool playerTurn = true;
    [Header("Time to wait in seconds")]
    public int timeToWait = 0;

    public OutputManager outputManager;

    public UIManager uIManager;

    public delegate void CombatCallback(int r);

    public CombatCallback combatCallback;

    private void Start()
    {
        StartCoroutine(setPlayer());
    }
    IEnumerator setPlayer()
    {
        yield return new WaitUntil(() => gameState.player != null);
        player = gameState.player;
    }

    public void startCombat(Enemy enemy, CombatCallback c)
    {
        this.combatCallback = c;
        this.enemy = enemy;
        nextTurn("Attack");

    }
    public void nextTurn(string action)
    {
        int result = 0;
        int dmgDone = 0;
        if (playerTurn)
        {
            
            switch (action)
            {
                case "Attack":
                    outputManager.outputMessage("You attack");
                    result = player.doDamage(this.enemy, out dmgDone);
                    if (result == 0)
                    {
                        outputManager.outputMessage("You didn't do any damage");
                        playerTurn = false;
                        combatCallback(-1);
                        StartCoroutine(waitBeforeComputerAction());
                    }
                    else if (result == 1)
                    {
                        outputManager.outputMessage("You hit them with " + dmgDone + " damage");
                        playerTurn = false;
                        StartCoroutine(waitBeforeComputerAction());
                        combatCallback(-1);
                    }
                    else if (result == 2)
                    {
                        outputManager.outputMessage("You hit with a fininshing blow");
                        enemy.die();
                        endCombat();
                        combatCallback(0);
                    }
                    else if (result == -1)
                    {
                        outputManager.outputMessage("That isn't an enemy");
                        endCombat();
                        combatCallback(1);
                    }
                    break;
                case "Defend":
                    if (!player.canDefend())
                    {
                        outputManager.outputMessage("You don't have a shield equiped");
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
                    endCombat();
                    combatCallback(2);
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
