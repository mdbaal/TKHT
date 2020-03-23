using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    public new string name;
    [TextArea]
    public string description;

    public Sprite sprite;
    private Inventory inventory = new Inventory();

    public Location[] neighbours;

    private ItemOBJ[] items;

    private Enemy[] enemies;

    private void Awake()
    {
        items = this.GetComponentsInChildren<ItemOBJ>();
        makeLocation();
    }

    public bool hasNeighbour(string l)
    {
        foreach(Location L in neighbours)
        {
            if (l.Equals(L.name)) return true;
        }
        return false;
    }

    private void makeLocation()
    {

        enemies = this.GetComponentsInChildren<Enemy>();

        foreach(ItemOBJ i in items)
        {
            inventory.addItem(i.item);
        }

        if (this.sprite != null)
        {
            SpriteRenderer sceneImg = this.gameObject.GetComponent<SpriteRenderer>();

            sceneImg.sprite = this.sprite;
        }

        this.leave();
    }

    public int takeItem(string item,ref Item i)
    {
        
        if (inventory.takeItem(item, ref i) == 0) return 0;
        //is it in scene?
        foreach(ItemOBJ iObj in items)
        {
            if(iObj.item == i)
            {
                iObj.GetComponent<SpriteRenderer>().enabled = false;
                return 1;
            }
        }
        return -1;
    }

    public int dropItem(Item item)
    {
        int result = inventory.addItem(item);
        if ( result== 1)
        {
            foreach (ItemOBJ iObj in items)
            {
                if (iObj.item == item)
                {
                    iObj.GetComponent<SpriteRenderer>().enabled = true;
                    return result;
                }
            }
        }
          return result;
    }

    public string getDescription()
    {
        return description;
    }

    public string listItems()
    {
        return inventory.ToString();
    }

    public void enter()
    {
        foreach(ItemOBJ item in items)
        {
            item.gameObject.SetActive(true);
        }
        this.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void leave()
    {
        foreach (ItemOBJ item in items)
        {
            item.gameObject.SetActive(false);
        }
        this.GetComponent<SpriteRenderer>().enabled = false;
    }

    public bool hasSpace()
    {
        return inventory.hasSpace();
    }

    public Enemy getEnemy(string enem)
    {
        foreach(Enemy e in enemies)
        {
            if (e.name.Equals(enem))
            {
                return e;
            }
        }
        return null;
    }

}
