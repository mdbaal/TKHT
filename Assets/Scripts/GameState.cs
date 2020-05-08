using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GameState", menuName = "Gamestate", order = 0)]
public class GameState : ScriptableObject
{
    public Player player = new Player();

    public List<QuestItem> questItemsCollected = new List<QuestItem>();

    public Location currentLocation;

    public bool inCombat = false;

    public bool readyForPlayerInput = false;

    public bool allQuestItemsCollected = false;

    public bool finishedTutorial = false;

    public bool isTrading = false;


    public int addToQuestItems(QuestItem item)
    {

        questItemsCollected.Add(item);

        if (questItemsCollected.Count == 4)
        {
            allQuestItemsCollected = true;
        }
        switch (item.name)
        {
            case "Crown":
                return 0;
            case "Boots":
                return 1;

            case "Dragon Egg":
                return 2;

            case "Pocket Chest":
                return 3;
        }

        return 0;
    }

    public void restart()
    {
        player = new Player();

        questItemsCollected = new List<QuestItem>();

        currentLocation = null;

        inCombat = false;

        readyForPlayerInput = false;

        allQuestItemsCollected = false;

        finishedTutorial = false;
    }

}
