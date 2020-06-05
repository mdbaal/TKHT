using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState
{
    [SerializeField]
    private static Player _player = new Player();
    [SerializeField]
    private static List<QuestItem> _questItemsCollected = new List<QuestItem>();
    [SerializeField]
    private static Location _currentLocation;
    [SerializeField]
    private static bool _inCombat = false;
    [SerializeField]
    private static bool _readyForPlayerInput = false;
    [SerializeField]
    private static bool _allQuestItemsCollected = false;
    [SerializeField]
    private static bool _finishedTutorial = false;
    [SerializeField]
    private static bool _isTrading = false;

    public static Player player { get => _player; set => _player = value; }
    public static List<QuestItem> questItemsCollected { get => _questItemsCollected; set => _questItemsCollected = value; }
    public static Location currentLocation { get => _currentLocation; set => _currentLocation = value; }
    public static bool inCombat { get => _inCombat; set => _inCombat = value; }
    public static bool readyForPlayerInput { get => _readyForPlayerInput; set => _readyForPlayerInput = value; }
    public static bool allQuestItemsCollected { get => _allQuestItemsCollected; set => _allQuestItemsCollected = value; }
    public static bool finishedTutorial { get => _finishedTutorial; set => _finishedTutorial = value; }
    public static bool isTrading { get => _isTrading; set => _isTrading = value; }


    public static void addToQuestItems(QuestItem item)
    {
        
        questItemsCollected.Add(item);

        if (questItemsCollected.Count == 4)
        {
            allQuestItemsCollected = true;
        }
        
    }

    public static void restart()
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
