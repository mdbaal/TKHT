using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    public new string name;
    [TextArea]
    public string description;

    public Sprite sprite;
    private Inventory _inventory = new Inventory();

    public Location[] neighbours;

    private ItemOBJ[] items;

    private EnemyOBJ[] enemies;
    private bool isMade = false;
    public bool PlayerVisited = false;

    public ItemOBJ[] Items { get => items; set => items = value; }
    public EnemyOBJ[] Enemies { get => enemies; set => enemies = value; }

    public bool hasNeighbour(string l)
    {
        foreach(Location L in neighbours)
        {
            if (l.Equals(L.name)) return true;
        }
        return false;
    }

    public void makeLocation(bool _leave,bool fromSave)
    {
        if (!fromSave)
        {
            items = this.GetComponentsInChildren<ItemOBJ>();
            enemies = this.GetComponentsInChildren<EnemyOBJ>();
        }

        foreach(ItemOBJ i in items)
        {
            _inventory.addItem(i.item);
        }

        if (this.sprite != null)
        {
            SpriteRenderer sceneImg = this.gameObject.GetComponent<SpriteRenderer>();

            sceneImg.sprite = this.sprite;
        }
        this.isMade = true;
        if(_leave) this.leave();
    }

    public int takeItem(string[] item,out Item i)
    {
        i = null;
        if (_inventory.takeItem(item,  out i) == 0) return 0;
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
        int result = _inventory.addItem(item);
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
        return _inventory.ToString();
    }

    public void enter()
    {
        if (isMade)
        {
            foreach (ItemOBJ item in items)
            {
                item.gameObject.SetActive(true);
            }
            foreach(EnemyOBJ enemy in enemies)
            {
                if (enemy != null)
                {
                    enemy.gameObject.SetActive(true);
                }
            }
            this.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            makeLocation(false,false);
            enter();
        }
    }

    public void leave()
    {
        foreach (ItemOBJ item in items)
        {
            item.gameObject.SetActive(false);
        }
        foreach (EnemyOBJ enemy in enemies)
        {
            if(enemy != null)
            enemy.gameObject.SetActive(false);
        }
        this.GetComponent<SpriteRenderer>().enabled = false;
    }

    public bool hasSpace()
    {
        return _inventory.hasSpace();
    }

    public Enemy getEnemy(string enem)
    {
        foreach(EnemyOBJ e in enemies)
        {
            if (e.name.Equals(enem))
            {
                return e.enemy;
            }
        }
        return null;
    }

    public ItemOBJ[] getInventoryItems()
    {
        return items;
    }

    public EnemyOBJ[] getEnemies()
    {
        List<EnemyOBJ> enemyOBJs = new List<EnemyOBJ>();

        foreach (EnemyOBJ eo in enemies)
        {
            if (eo != null) enemyOBJs.Add(eo);
        }
        return enemyOBJs.ToArray();
    }

}
