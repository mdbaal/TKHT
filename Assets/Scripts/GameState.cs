using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GameState", menuName = "Gamestate", order = 0)]
public class GameState : ScriptableObject
{
    public Player player;

    public List<Item> questItemsCollected =  new List<Item>();

    public Location currentLocation;

    public bool inCombat = false;

    public bool readyForPlayerInput = true;

    public bool allQuestItemsCollected = false;


    public int addToQuestItems(Item item)
    {

        questItemsCollected.Add(item);

        if(questItemsCollected.Count == 4)
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

}
