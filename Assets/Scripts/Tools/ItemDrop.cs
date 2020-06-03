using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDrop : MonoBehaviour
{
    private BoxCollider2D dropField;

    private float width;
    private float height;
    private float xoffset;
    private float yoffset;

    private int amountOfItemsPossibleX = 0;
    private int amountOfItemsPossibleY = 0;
    private int _amountOfItemsTotal = 0;
    private float itemSpriteSizeInUnits = 0;
    [SerializeField]
    private List<Vector3> dropPositionsCalculated = new List<Vector3>();
    [SerializeField]
    private List<Vector3> dropPositionsAvailable = new List<Vector3>();

    public int amountOfItemsTotal { get => _amountOfItemsTotal; set => _amountOfItemsTotal = value; }

    public void calculateValues()
    {
        this.dropField = this.GetComponent<BoxCollider2D>();
        width = dropField.size.x;
        height = dropField.size.y;

        xoffset = dropField.offset.x;
        yoffset = dropField.offset.y;

        itemSpriteSizeInUnits = 1.28f;

        calculateAmount();
        calculatePositions();
    }

    private void calculateAmount()
    {
        amountOfItemsPossibleX = Mathf.FloorToInt(width / itemSpriteSizeInUnits);
        amountOfItemsPossibleY = Mathf.FloorToInt(height / itemSpriteSizeInUnits);

        _amountOfItemsTotal = amountOfItemsPossibleX * amountOfItemsPossibleY;
    }

    private void calculatePositions()
    {
        Vector3 startPosition = new Vector3(dropField.bounds.center.x - (dropField.bounds.size.x / 2) + (itemSpriteSizeInUnits / 2), dropField.bounds.center.y - (dropField.bounds.size.y / 2) + (itemSpriteSizeInUnits / 2), 0);


        for (int y = 0; y < amountOfItemsPossibleY; y++)
        {
            for (int x = 0; x < amountOfItemsPossibleX; x++)
            {

                dropPositionsCalculated.Add(new Vector3(startPosition.x + (itemSpriteSizeInUnits * x), startPosition.y + (itemSpriteSizeInUnits * y), 0));
            }

        }
        foreach (Vector3 v in dropPositionsCalculated)
            dropPositionsAvailable.Add(v);
    }


    public int dropItem(Item item,out ItemOBJ outItemObj)
    {
        outItemObj = null;
        if (item == null) return 0;


        int randomIndex = Random.Range(0, dropPositionsAvailable.Count-1);

        ItemOBJ io = Instantiate<ItemOBJ>(Resources.Load<ItemOBJ>("Items/Prefabs/" + item.name), dropPositionsAvailable[randomIndex], Quaternion.identity,this.transform);
        io.item = item;
        io.droppedIndex = randomIndex;
        outItemObj = io;
        dropPositionsAvailable.RemoveAt(randomIndex);

        return 1;
    }

    public void itemPickedUp(int i)
    {
        if (i == -1) return;
        dropPositionsAvailable.Add(dropPositionsCalculated[i]);
    }


}
