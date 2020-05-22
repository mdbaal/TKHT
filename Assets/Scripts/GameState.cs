using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GameState", menuName = "Gamestate", order = 0)]
public class GameState : ScriptableObject
{
    [SerializeField]
    private Player _player = new Player();
    [SerializeField]
    private List<QuestItem> _questItemsCollected = new List<QuestItem>();
    [SerializeField]
    private Location _currentLocation;
    [SerializeField]
    private bool _inCombat = false;
    [SerializeField]
    private bool _readyForPlayerInput = false;
    [SerializeField]
    private bool _allQuestItemsCollected = false;
    [SerializeField]
    private bool _finishedTutorial = false;
    [SerializeField]
    private bool _isTrading = false;

    public Player player { get => _player; set => _player = value; }
    public List<QuestItem> questItemsCollected { get => _questItemsCollected; set => _questItemsCollected = value; }
    public Location currentLocation { get => _currentLocation; set => _currentLocation = value; }
    public bool inCombat { get => _inCombat; set => _inCombat = value; }
    public bool readyForPlayerInput { get => _readyForPlayerInput; set => _readyForPlayerInput = value; }
    public bool allQuestItemsCollected { get => _allQuestItemsCollected; set => _allQuestItemsCollected = value; }
    public bool finishedTutorial { get => _finishedTutorial; set => _finishedTutorial = value; }
    public bool isTrading { get => _isTrading; set => _isTrading = value; }

    public void addToQuestItems(QuestItem item)
    {

        questItemsCollected.Add(item);

        if (questItemsCollected.Count == 4)
        {
            allQuestItemsCollected = true;
        }
        
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
