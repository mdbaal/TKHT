﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Game State")]
    [SerializeField]
    private GameState gameState;
    [Header("Output Scrollrect")]
    [SerializeField]
    private ScrollRect textScroll;
    [Header("Inventory & Equipment")]
    [SerializeField]
    private List<GameObject> inventorySlots = new List<GameObject>();

    [SerializeField]
    private List<GameObject> equipmentSlots = new List<GameObject>();

    [Header("Objectives texts")]
    [SerializeField]
    private Text[] ObjectivesTexts;


    [Header("Player health")]
    [SerializeField]
    private GameObject playerHealth;

    private int fullHealth = 0;

    [Header("Player Gold")]
    [SerializeField]
    private Text goldText;

    [Header("Tutorial pop-up")]
    [SerializeField]
    private GameObject tutorialPopup;
    [SerializeField]
    private Text tutorialNumber;
    [SerializeField]
    public Text[] tutorialTexts = new Text[] { };

    private int tutorialIndex = 0;

    [Header("Endscreen UI")]
    [SerializeField]
    private GameObject Endscreen;
    [SerializeField]
    private Text EndscreenTitle;
    [SerializeField]
    private Text EndscreenText;

    [Header("Minimap")]
    [SerializeField]
    private Image mapImage;
    [SerializeField]
    private Sprite[] mapImages;
    [SerializeField]
    private Image[] mapPoints;

    private int currentActivePoint = 0;


    private void Start()
    {
        StartCoroutine(setPlayerValues());
    }
    IEnumerator setPlayerValues()
    {
        yield return new WaitUntil(() => gameState.player != null);
        fullHealth = gameState.player.health;
        updatePlayerHealth(gameState.player);
        updateGold();
    }


    public void outputToBottom()
    {
        Canvas.ForceUpdateCanvases();

        textScroll.verticalNormalizedPosition = 0;
    }

    public void addToPlayerInventory(Item item)
    {
        foreach (GameObject i in inventorySlots)
        {
            Image img = i.GetComponent<Image>();
            if (img.sprite == null)
            {
                img.sprite = item.sprite;
                img.enabled = true;
                return;
            }
        }
    }

    public void removeFromPlayerInventory(Item item)
    {

        foreach (GameObject i in inventorySlots)
        {
            Image img = i.GetComponent<Image>();

            if (img.sprite == item.sprite)
            {
                img.sprite = null;
                img.enabled = false;
                return;
            }
        }
    }

    public void addToEquiped(Item item)
    {

        removeFromPlayerInventory(item);
        if (item.GetType() == typeof(Weapon))
        {
            Image img = equipmentSlots[0].GetComponentInChildren<Image>();
            img.sprite = item.sprite;
            img.enabled = true;
        }
        else if (item.GetType() == typeof(Shield))
        {
            Image img = equipmentSlots[1].GetComponentInChildren<Image>();
            img.sprite = item.sprite;
            img.enabled = true;
        }
    }

    public void removeFromEquiped(Item item)
    {
        addToPlayerInventory(item);
        if (item.GetType() == typeof(Weapon))
        {
            Image img = equipmentSlots[0].GetComponentInChildren<Image>();
            img.sprite = null;
            img.enabled = false;
        }
        else if (item.GetType() == typeof(Shield))
        {
            Image img = equipmentSlots[1].GetComponentInChildren<Image>();
            img.sprite = null;
            img.enabled = false;
        }
    }

    public void updatePlayerHealth(Player player)
    {
        if (fullHealth == 0)
        {
            fullHealth = player.maxHealth;
        }

        Image healthImg = playerHealth.GetComponentsInChildren<Image>()[1];
        Text healthText = playerHealth.GetComponentInChildren<Text>();

        if (player.health < 0)
        {
            healthText.text = "Health: 0";
            healthImg.rectTransform.localScale = new Vector3(0, 1, 1);
        }
        else
        {
            healthText.text = "Health: " + player.health.ToString();
            healthImg.rectTransform.localScale = new Vector3((float)fullHealth / 100 * player.health, 1, 1);
        }

    }

    public void UpdateObjectiveText(QuestItem item)
    {
        switch (item.name)
        {
            case "Crown":
                ObjectivesTexts[0].color = new Color(0, 0, 0, 1);
                ObjectivesTexts[0].text = "Collected Crown";
                break;
            case "Boots":
                ObjectivesTexts[1].color = new Color(0, 0, 0, 1);
                ObjectivesTexts[1].text = "Collected Boots";
                break;
            case "Dragon Egg":
                ObjectivesTexts[2].color = new Color(0, 0, 0, 1);
                ObjectivesTexts[2].text = "Collected Draggon Egg";
                break;
            case "Pocket Chest":
                ObjectivesTexts[3].color = new Color(0, 0, 0, 1);
                ObjectivesTexts[3].text = "Collected Pocket Chest";
                break;
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            nextTutorial();


        if (Input.GetKeyDown(KeyCode.Backspace))
            prevTutorial();
    }

    public void startTutorial()
    {
        tutorialPopup.SetActive(true);

        tutorialNumber.text = (tutorialIndex + 1).ToString() + "/" + tutorialTexts.Length.ToString();
        tutorialTexts[tutorialIndex].enabled = true;


    }

    public void nextTutorial()
    {
        if (tutorialIndex < tutorialTexts.Length - 1)
        {
            tutorialTexts[tutorialIndex].enabled = false;
            tutorialIndex++;
            tutorialNumber.text = (tutorialIndex + 1).ToString() + "/" + tutorialTexts.Length.ToString();
            tutorialTexts[tutorialIndex].enabled = true;
        }
        else
        {
            endTutorial();
        }
    }

    public void prevTutorial()
    {
        if (tutorialIndex > 0)
        {
            tutorialTexts[tutorialIndex].enabled = false;
            tutorialIndex--;
            tutorialNumber.text = (tutorialIndex + 1).ToString() + "/" + tutorialTexts.Length.ToString();
            tutorialTexts[tutorialIndex].enabled = true;
        }
    }

    public void endTutorial()
    {
        tutorialPopup.SetActive(false);

        gameState.finishedTutorial = true;
        gameState.readyForPlayerInput = true;
    }

    public void showEndscreen(int i)
    {
        Endscreen.SetActive(true);
        if (i == 0)
        {
            EndscreenTitle.text = "Game Over";
            EndscreenText.text = "You died. \nPress <b>Enter</b> to start again or <b>Esc</b> to quit.";
        }
        else
        {
            EndscreenTitle.text = "Victory";
            EndscreenText.text = "You did it! You got all the items and now live a rich life of fortune. \nPress <b>Enter</b> to start again or <b>Esc</b> to quit.";
        }
    }

    public void closeEndScreen()
    {
        Endscreen.SetActive(false);
        EndscreenTitle.text = string.Empty;
        EndscreenText.text = string.Empty;
    }

    public void UpdateMinimap(string loc)
    {
        switch (loc)
        {
            case "City Gate":
                mapPoints[currentActivePoint].enabled = false;
                mapPoints[0].enabled = true;
                currentActivePoint = 0;
                break;
            case "Store":
                mapPoints[currentActivePoint].enabled = false;
                mapPoints[1].enabled = true;
                currentActivePoint = 1;
                break;
            case "Smithy":
                mapPoints[currentActivePoint].enabled = false;
                mapPoints[2].enabled = true;
                currentActivePoint = 2;
                break;
            case "Castle Gate":
                mapPoints[currentActivePoint].enabled = false;
                mapPoints[3].enabled = true;
                currentActivePoint = 3;

                if (mapImage.sprite != mapImages[0]) mapImage.sprite = mapImages[0];
                break;

            case "Castle Hall":
                mapPoints[currentActivePoint].enabled = false;

                mapPoints[4].enabled = true;
                currentActivePoint = 4;

                if (mapImage.sprite != mapImages[1]) mapImage.sprite = mapImages[1];
                break;
            case "Throne Room":
                mapPoints[currentActivePoint].enabled = false;
                mapPoints[5].enabled = true;
                currentActivePoint = 5;
                break;
            case "Kitchen Hall":
                mapPoints[currentActivePoint].enabled = false;
                mapPoints[6].enabled = true;
                currentActivePoint = 6;
                break;
            case "Chamber Hall":
                mapPoints[currentActivePoint].enabled = false;
                mapPoints[7].enabled = true;
                currentActivePoint = 7;
                break;
            case "Treasury":
                mapPoints[currentActivePoint].enabled = false;
                mapPoints[8].enabled = true;
                currentActivePoint = 8;
                break;
            case "Kitchen":
                mapPoints[currentActivePoint].enabled = false;
                mapPoints[9].enabled = true;
                currentActivePoint = 9;
                break;
            case "Royal Chambers":
                mapPoints[currentActivePoint].enabled = false;
                mapPoints[10].enabled = true;
                currentActivePoint = 10;
                break;
        }
    }

    public void updateGold()
    {
        goldText.text = "Gold: " + gameState.player.gold.ToString();
    }
}
