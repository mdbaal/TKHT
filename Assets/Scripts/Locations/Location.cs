using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    [SerializeField]
    private string _name;
    [TextArea]
    [SerializeField]
    private string _description;

    [SerializeField]
    private Sprite _sprite;

    private Inventory _inventory = new Inventory();

    [SerializeField]
    private Location[] _neighbours;

    [Header("All scene OBJs")]
    [SerializeField]
    private List<ItemOBJ> _items = new List<ItemOBJ>();
    [SerializeField]
    private EnemyOBJ[] _enemies;
    [SerializeField]
    private TraderOBJ _trader;

    private bool isMade = false;
    [SerializeField]
    private bool _playerVisited = false;

    //Enemy settings of scene
    public bool allEnemiesDeadToContinue = false;
    private bool _allEnemiesDead = false;
    private int enemiesDead = 0;
    private int enemiesInScene = 0;

    private ItemDrop itemDropTool;

    public List<ItemOBJ> items { get => _items; set => _items = value; }
    public EnemyOBJ[] enemies { get => _enemies; set => _enemies = value; }
    public TraderOBJ trader { get => _trader; set => _trader = value; }
    public bool allEnemiesDead { get => _allEnemiesDead; set => _allEnemiesDead = value; }
    public string name { get => _name; set => _name = value; }
    public string description { get => _description;}
    public Sprite sprite { get => _sprite;}
    public Location[] neighbours { get => _neighbours; }
    public bool playerVisited { get => _playerVisited; set => _playerVisited = value; }

    //Is the location you try to access a neighbour
    public bool hasNeighbour(string l)
    {
        foreach (Location L in neighbours)
        {
            if (l.Equals(L.name)) return true;
        }
        return false;
    }

    //At start of game build scene
    public void makeLocation(bool _leave, bool fromSave)
    {
        if (!fromSave)
        {
            _items.AddRange(this.GetComponentsInChildren<ItemOBJ>());
            _enemies = this.GetComponentsInChildren<EnemyOBJ>();
            _trader = this.GetComponentInChildren<TraderOBJ>();
        }

        this.itemDropTool = this.GetComponent<ItemDrop>();
        itemDropTool.calculateValues();
        this._inventory.setSpace(itemDropTool.amountOfItemsTotal);

        foreach (ItemOBJ i in _items)
        {
            _inventory.addItem(i.item);
        }

        if (this.sprite != null)
        {
            SpriteRenderer sceneImg = this.gameObject.GetComponent<SpriteRenderer>();

            sceneImg.sprite = this.sprite;
        }
        enemiesInScene = enemies.Length;

        this.isMade = true;
        if (_leave) this.leave();
    }

    //Take item from scene
    public int takeItem(string[] item, out Item i)
    {
        i = null;
        if (this.allEnemiesDeadToContinue && !this.allEnemiesDead)
        {
            return -1;
        }
        if (_inventory.takeItem(item, out i) == 0) return 0;
        //is it in scene?
        foreach (ItemOBJ iObj in _items)
        {
            if (iObj.item == i)
            {
                itemDropTool.itemPickedUp(iObj.droppedIndex);
                items.Remove(iObj);
                Destroy(iObj.gameObject);
                return 1;
            }
        }
        return -1;
    }

    //Drop item in scene
    public int dropItem(Item item)
    {
        ItemOBJ outItemObj = null;
        if (item == null) return 0;
        if (item.GetType() == typeof(QuestItem)) return -1;
        if (!hasSpace()) return -2;
        int result = itemDropTool.dropItem(item,out outItemObj);
        if (result == 1)
        {
            result = _inventory.addItem(item);
            items.Add(outItemObj);
                return result;
        }
        return result;
    }

    //Get item without removing it.
    public Item getItem(string[] item)
    {
        return _inventory.getItem(item);
    }

    //Get the scene description
    public string getDescription()
    {
        return description;
    }
    //Get list of all items in scene
    public string listItems()
    {
        return _inventory.ToString();
    }
    //Get list of all npc's in scene
    public string getNpcs()
    {
        string npcs = "";
        foreach (EnemyOBJ e in enemies)
        {
            if(e != null)
            npcs += "  - " + e.name + "\n";
        }

        if(trader != null)
        npcs += " - " + trader.name + "\n";

        return npcs;
    }

    //Enter scene and activate objects
    public void enter()
    {
        if (isMade)
        {
            foreach (ItemOBJ item in _items)
            {
                item.gameObject.SetActive(true);
            }
            foreach (EnemyOBJ enemy in _enemies)
            {
                if (enemy != null)
                {
                    enemy.gameObject.SetActive(true);
                }
            }

            if (trader != null)
            {
                trader.gameObject.SetActive(true);
            }

            this.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            makeLocation(false, false);
            enter();
        }
    }
    //Leave scene and deactivate objects
    public void leave()
    {
        foreach (ItemOBJ item in _items)
        {
            item.gameObject.SetActive(false);
        }
        foreach (EnemyOBJ enemy in _enemies)
        {
            if (enemy != null) enemy.gameObject.SetActive(false);
        }

        if (trader != null)
        {
            trader.gameObject.SetActive(false);
        }

        this.GetComponent<SpriteRenderer>().enabled = false;
    }

    //Is there room to drop an item
    public bool hasSpace()
    {
        return _inventory.hasSpace();
    }


    //Get a specific enemy
    public Enemy getEnemy(string enem)
    {
        foreach (EnemyOBJ e in _enemies)
        {
            if (e != null)
            {
                if (e.name.Equals(enem))
                {
                    return e.enemy;
                }
            }
        }
        return null;
    }
    //Get all item objects as an array
    public ItemOBJ[] getInventoryItems()
    {
        return _items.ToArray();
    }
    //Get all enemy objects
    public EnemyOBJ[] getEnemies()
    {
        List<EnemyOBJ> enemyOBJs = new List<EnemyOBJ>();

        foreach (EnemyOBJ eo in _enemies)
        {
            if (eo != null) enemyOBJs.Add(eo);
        }
        return enemyOBJs.ToArray();
    }
    //Get all trader ojbects
    public TraderOBJ getTraderOBJ()
    {
        return trader;
    }
    public Trader getTrader()
    {
        if (trader == null) return null;
        return trader.trader;
    }
    //Add one to dead enemies count
    public void enemyDied()
    {
        this.enemiesDead++;
    }
    //Check if all enemies are dead
    public void checkAllDead()
    {
        if (enemiesDead == enemiesInScene) allEnemiesDead = true;
    }

    public void clearInventory()
    {
        this._inventory.clearInventory();
    }

}
