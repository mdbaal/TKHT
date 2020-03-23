using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private Player player = null;
    private Enemy enemy = null;

    private bool playerTurn = true;
    public int endcode = -1;
    [Header("TIme to wait in seconds")]
    public int timeToWait = 0;

    public OutputManager outputManager;

    public UIManager uIManager;

    public void setPlayer(ref Player player)
    {
        this.player = player;
    }

    public void startCombat(Enemy enemy)
    { 
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
                    result = player.doDamage(ref this.enemy, out dmgDone);
                    if (result == 0)
                    {
                        outputManager.outputMessage("You didn't do any damage");
                        playerTurn = false;
                        StartCoroutine(waitBeforeComputerAction());
                    }
                    else if (result == 1)
                    {
                        outputManager.outputMessage("You hit them with " + dmgDone + " damage");
                        playerTurn = false;
                        StartCoroutine(waitBeforeComputerAction());
                    }
                    else if (result == 2)
                    {
                        outputManager.outputMessage("You hit with a fininshing blow");
                        Destroy(enemy.gameObject);
                        endCombat(0);
                    }
                    else if (result == -1)
                    {
                        outputManager.outputMessage("That isn't an enemy");
                        endCombat(1);
                    }
                    break;
                case "Defend":
                    if (!player.canDefend())
                    {
                        outputManager.outputMessage("You don't have a shield equiped");
                        playerTurn = false;
                        StartCoroutine(waitBeforeComputerAction());
                        break;
                    }
                    outputManager.outputMessage("You defend");
                    player.defending = true;
                    StartCoroutine(waitBeforeComputerAction());
                    break;
                case "Flee":
                    outputManager.outputMessage("You flee from the battle");
                    endCombat(2);
                    break;
            }
        }
        else
        {
            outputManager.outputMessage(enemy.name + " attacks");
            result = enemy.doDamage(ref player, out dmgDone);

            if (result == -1 || result == 0)
            {
                outputManager.outputMessage("You didn't take any damage");
            } else if (result == 1)
            {
                outputManager.outputMessage("You took " + dmgDone + " damage");
                uIManager.updatePlayerHealth(ref player);
            } else if (result == 2) {
                outputManager.outputMessage("You have been killed");
                uIManager.updatePlayerHealth(ref player);
                player.isAlive = false;
                endCombat(3);
            }else if (result == 3)
            {
                outputManager.outputMessage("You fully blocked the attack");
            }

            playerTurn = true;
        }
    }
    public void endCombat(int _endcode)
    {
        this.enemy = null;
        this.endcode = _endcode;
    }

    IEnumerator waitBeforeComputerAction()
    {
        yield return new WaitForSeconds(timeToWait);
        nextTurn("");
    }
}
