using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Game State")]
    public GameState gameState;
    [Header("Output Scrollrect")]
    public ScrollRect textScroll;
    [Header("Inventory & Equipment")]
    public List<GameObject> inventorySlots = new List<GameObject>();

    public List<GameObject> equipmentSlots = new List<GameObject>();

    [Header("Objectives texts")]
    public Text[] ObjectivesTexts;
    

    [Header("Player health")]
    public GameObject playerHealth;
    public int fullHealth = 0;

    [Header("Tutorial pop-up")]
    public GameObject tutorialPopup;
    Text[] tutorialTexts;
    int tutorialIndex = 1;

    [Header("Endscreen UI")]
    public GameObject Endscreen;
    public Text EndscreenTitle;
    public Text EndscreenText;


    private void Start()
    {
        StartCoroutine(setPlayerValues());
    }
    IEnumerator setPlayerValues()
    {
        yield return new WaitUntil(() => gameState.player != null);
        fullHealth = gameState.player.health;
        updatePlayerHealth( gameState.player);
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
           if (img.sprite == null) {
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
        else if(item.GetType() == typeof(Shield))
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
        else if(item.GetType() == typeof(Shield))
        {
            Image img = equipmentSlots[1].GetComponentInChildren<Image>();
            img.sprite = null;
            img.enabled = false;
        }
    }

    public void updatePlayerHealth( Player player)
    {
        if(fullHealth == 0)
        {
            fullHealth = player.maxHealth;
        }

        Image healthImg = playerHealth.GetComponentsInChildren<Image>()[1];
        Text healthText = playerHealth.GetComponentInChildren<Text>();

        if (player.health < 0)
        {
            healthText.text = "Health: 0";
            healthImg.rectTransform.localScale = new Vector3(0, 1,1);
        }
        else
        {
            healthText.text = "Health: " + player.health.ToString();
            healthImg.rectTransform.localScale = new Vector3((float)fullHealth / 100 * player.health, 1,1);
        }

    }

    public void UpdateObjectiveText(int index)
    {
        ObjectivesTexts[index].color = new Color(0, 0, 0, 1);
    }

    IEnumerator tutorialInput()
    {
        yield return new WaitForEndOfFrame();

        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Return));
        nextTutorial();
    }

    public void startTutorial()
    {
        tutorialPopup.SetActive(true);

        tutorialTexts = tutorialPopup.GetComponentsInChildren<Text>();

        tutorialTexts[tutorialIndex].enabled = true;

        StartCoroutine(tutorialInput());
    }

    public void nextTutorial()
    {
        if(tutorialIndex < tutorialTexts.Length-1)
        {
            tutorialTexts[tutorialIndex].enabled = false;
            tutorialIndex++;
            tutorialTexts[tutorialIndex].enabled = true;
            StartCoroutine(tutorialInput());
        }
        else
        {
            endTutorial();
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
        if(i == 0)
        {
            EndscreenTitle.text = "Game Over";
            EndscreenText.text = "You died. \nPress <b>Enter</b> to start again or <b>Esc</b> to quit.";
        }else
        {
            EndscreenTitle.text = "Victory";
            EndscreenText.text = "You did it! You got all the items and now live a rich live of fortune. \nPress <b>Enter</b> to start again or <b>Esc</b> to quit.";
        }
    }

    public void closeEndScreen()
    {
        Endscreen.SetActive(false);
        EndscreenTitle.text = string.Empty;
        EndscreenText.text = string.Empty;
    }
}
