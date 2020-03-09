using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GameState", menuName = "Gamestate", order = 0)]
public class GameState : ScriptableObject
{
    public Player player;

    public int questItemsCollected = 0;

    public string CurrentLocation;

    public bool inCombat = false;

}
