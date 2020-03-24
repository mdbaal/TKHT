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


    [Header("Player health")]
    public GameObject playerHealth;
    public int fullHealth = 0;

    private void Start()
    {
        StartCoroutine(setPlayerValues());
    }
    IEnumerator setPlayerValues()
    {
        yield return new WaitUntil(() => gameState.player != null);
        fullHealth = gameState.player.Health;
        updatePlayerHealth(ref gameState.player);
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
        if (item.equipLeft)
        {
            Image img = equipmentSlots[0].GetComponentInChildren<Image>();
           img.sprite = item.sprite;
            img.enabled = true;
        }
        else
        {
            Image img = equipmentSlots[1].GetComponentInChildren<Image>();
            img.sprite = item.sprite;
            img.enabled = true;
        }
    }

    public void removeFromEquiped(Item item)
    {
        addToPlayerInventory(item);
        if (item.equipLeft)
        {
            Image img = equipmentSlots[0].GetComponentInChildren<Image>();
            img.sprite = null;
            img.enabled = false;
        }
        else
        {
            Image img = equipmentSlots[1].GetComponentInChildren<Image>();
            img.sprite = null;
            img.enabled = false;
        }
    }

    public void updatePlayerHealth(ref Player player)
    {
        if(fullHealth == 0)
        {
            fullHealth = player.Health;
        }

        Image healthImg = playerHealth.GetComponentsInChildren<Image>()[1];
        Text healthText = playerHealth.GetComponentInChildren<Text>();

        if (player.Health < 0)
        {
            healthText.text = "Health: 0";
            healthImg.rectTransform.localScale = new Vector3(0, 1,1);
        }
        else
        {
            healthText.text = "Health: " +player.Health.ToString();
            healthImg.rectTransform.localScale = new Vector3((float)fullHealth / 100 * player.Health, 1,1);
        }

    }
}
