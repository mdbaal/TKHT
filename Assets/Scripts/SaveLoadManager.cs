using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.U2D;

public static class SaveLoadManager
{

    private static bool loadedResources = false;

    private static Dictionary<string, Item> itemListFromResources = new Dictionary<string, Item>();
    private static Dictionary<string, ItemOBJ> itemObjListFromResources = new Dictionary<string, ItemOBJ>();
    private static Dictionary<string, EnemyOBJ> enemyObjListFromResources = new Dictionary<string, EnemyOBJ>();
    private static Dictionary<string, TraderOBJ> traderObjListFromResources = new Dictionary<string, TraderOBJ>();
    private static SpriteAtlas characterSprites;


    private static void getObjectsFromResources()
    {
        if (loadedResources) return;

        Item[] items = Resources.LoadAll<Item>("Items/");

        foreach (Item i in items)
        {
            if (i != null)
                itemListFromResources.Add(i.name, i);
        }

        ItemOBJ[] itemOBJs = Resources.LoadAll<ItemOBJ>("Items/Prefabs/");

        foreach (ItemOBJ i in itemOBJs)
        {
            if (i != null)
                itemObjListFromResources.Add(i.name, i);
        }

        EnemyOBJ[] enemyOBJs = Resources.LoadAll<EnemyOBJ>("Npcs/Enemies/");
        foreach (EnemyOBJ i in enemyOBJs)
        {
            if (i != null)
                enemyObjListFromResources.Add(i.name, i);
        }

        TraderOBJ[] traderOBJs = Resources.LoadAll<TraderOBJ>("Npcs/Traders/");

        foreach (TraderOBJ i in traderOBJs)
        {
            if (i != null)
                traderObjListFromResources.Add(i.name, i);
        }

        characterSprites = Resources.Load<SpriteAtlas>("Sprites/Characters");

        loadedResources = true;
    }

    [System.Serializable]
    public struct GameStateData
    {
        public PlayerData player;
        public ItemData[] questItemsCollected;
        public bool finishedTutorial;
        public string currentLocation;
    }

    [System.Serializable]
    public struct PlayerData
    {
        public int health;
        public int maxHealth;
        public string weapon;
        public string shield;
        public ItemData[] inventory;
        public int gold;
    }

    [System.Serializable]
    public struct Locations
    {
        public LocationData[] locations;
    }

    [System.Serializable]
    public struct LocationData
    {
        public string locationName;
        public ItemData[] items;
        public EnemyData[] enemies;
        public TraderData trader;
    }

    [System.Serializable]
    public struct EnemyData
    {
        public string name;
        public int maxHealth;
        public string weapon;
        public string shield;
        public string sprite;
        public float[] position;
    }

    [System.Serializable]
    public struct TraderData
    {
        public string name;
        public int maxHealth;
        public string weapon;
        public string sprite;
        public float[] position;
        public int gold;
        public ItemData[] stock;
    }

    [System.Serializable]
    public struct ItemData
    {
        public string name;
        public float[] position;
    }

    #region save
    //SAVE GAME TO JSON IN TXT FILE
    public static void save(LocationsMap locationsMap)
    {
        //GameState data
        GameStateData gameStateData = new GameStateData();

        gameStateData.finishedTutorial = GameState.finishedTutorial;

        List<ItemData> idl = new List<ItemData>();

        foreach (Item qi in GameState.questItemsCollected)
        {
            ItemData id = new ItemData();
            id.name = qi.name;
            id.position = new float[] { 0, 0, 0 };
        }

        gameStateData.questItemsCollected = idl.ToArray();

        Player _player = GameState.player;

        //PLAYER DATA
        PlayerData playerData = new PlayerData();

        playerData.health = _player.health;
        playerData.maxHealth = _player.maxHealth;
        if (_player.weapon != null)
            playerData.weapon = _player.weapon.name;
        if (_player.shield != null)
            playerData.shield = _player.shield.name;

        List<ItemData> ids = new List<ItemData>();

        foreach (Item s in _player.getInventoryItems())
        {
            ItemData id = new ItemData();

            id.name = s.name;
            id.position = new float[] { 0, 0, 0 };
            ids.Add(id);
        }

        playerData.inventory = ids.ToArray();

        gameStateData.currentLocation = locationsMap.getLocationName();

        playerData.gold = _player.gold;

        gameStateData.player = playerData;

        //LOCATION DATA
        Locations locations = new Locations();
        Location[] locationsFromMap = locationsMap.getAllLocations();
        List<LocationData> lds = new List<LocationData>();
        foreach (Location l in locationsFromMap)
        {

            if (l.playerVisited)
            {
                LocationData ld = new LocationData();
                ld.locationName = l.name;

                ids = new List<ItemData>();

                foreach (ItemOBJ s in l.getInventoryItems())
                {
                    if (s.GetComponent<SpriteRenderer>().enabled)
                    {
                        ItemData id = new ItemData();

                        id.name = s.name;
                        id.position = new float[] { s.transform.position.x, s.transform.position.y, s.transform.position.z };
                        ids.Add(id);
                    }
                }

                ld.items = ids.ToArray();

                List<EnemyData> edList = new List<EnemyData>();

                foreach (EnemyOBJ e in l.getEnemies())
                {
                    EnemyData ed = new EnemyData();
                    ed.name = e.name;
                    ed.maxHealth = e.maxHealth;
                    if (e.weapon != null) ed.weapon = e.weapon.name;
                    if (e.shield != null) ed.shield = e.shield.name;
                    ed.sprite = e.sprite.name;

                    ed.position = new float[] { e.transform.position.x, e.transform.position.y, e.transform.position.z };

                    edList.Add(ed);
                }
                ld.enemies = edList.ToArray();


                if (l.trader != null)
                {
                    TraderData td = new TraderData();
                    TraderOBJ trader = l.getTraderOBJ();

                    td.name = trader.name;
                    td.maxHealth = trader.maxHealth;
                    td.weapon = trader.weapon.name;
                    td.sprite = trader.sprite.name;
                    td.gold = trader.trader.gold;
                    td.position = new float[] { trader.transform.position.x, trader.transform.position.y, trader.transform.position.z };

                    idl.Clear();

                    foreach (Item i in trader.stock)
                    {
                        ItemData id = new ItemData();
                        id.name = i.name;
                        id.position = new float[] { 0, 0, 0 };
                        idl.Add(id);
                    }

                    td.stock = idl.ToArray();

                    ld.trader = td;
                }
                lds.Add(ld);
            }

        }
        locations.locations = lds.ToArray();

        string JsonGameState = JsonUtility.ToJson(gameStateData);
        string JsonStringLocations = JsonUtility.ToJson(locations);

        if (!Directory.Exists("Gamesave/"))
        {
            try
            {
                Directory.CreateDirectory("Gamesave/");
            }
            catch (System.Exception e)
            {
                Debug.Log("Creation failed: " + e.ToString());
            }
        }

        using (StreamWriter writer = new StreamWriter(@"Gamesave/save.json"))
        {
            writer.WriteLine(JsonGameState);
            writer.WriteLine(JsonStringLocations);
        }
    }
    #endregion

    #region load
    //LOAD GAME FROM SAVE FILE
    public static void load(LocationsMap locationsMap, UIManager uIManager)
    {
        if (!Directory.Exists("Gamesave/"))
        {
            return;
        }
        getObjectsFromResources();
        string JsonGameState = "";
        string JsonStringLocations = "";
        using (StreamReader reader = new StreamReader("Gamesave/save.json"))
        {
            JsonGameState = reader.ReadLine();
            JsonStringLocations = reader.ReadLine();
        }

        GameStateData gameStateData = JsonUtility.FromJson<GameStateData>(JsonGameState);
        Locations locations = JsonUtility.FromJson<Locations>(JsonStringLocations);
        //Set GameState values from data
        GameState.finishedTutorial = gameStateData.finishedTutorial;

        foreach (ItemData id in gameStateData.questItemsCollected)
        {

            GameState.addToQuestItems((QuestItem)itemListFromResources[id.name]);
        }

        //CREATE PLAYER FROM SAVE DATA
        Player _player = new Player();

        _player.health = gameStateData.player.health;
        _player.maxHealth = gameStateData.player.maxHealth;
        _player.gold = gameStateData.player.gold;

        if (gameStateData.player.weapon != string.Empty && gameStateData.player.weapon != "")
            _player.weapon = (Weapon)itemListFromResources[gameStateData.player.weapon];
        if (gameStateData.player.weapon != string.Empty && gameStateData.player.shield != "")
            _player.shield = (Shield)itemListFromResources[gameStateData.player.shield];

        foreach (ItemData id in gameStateData.player.inventory)
        {
            
            _player.giveItem(itemListFromResources[id.name]);
        }

        GameState.player = _player;

        //EDIT EXISTING LOCATIONS WITH SAVE DATA

        foreach (LocationData ld in locations.locations)
        {
            Location l = locationsMap.getLocation(ld.locationName);

            l.playerVisited = true;

            List<ItemOBJ> itemObjs = new List<ItemOBJ>();

            foreach (ItemOBJ io in l.items)
            {
                GameObject.Destroy(io.gameObject);
            }

            l.items.Clear();

            foreach (ItemData id in ld.items)
            {
                ItemOBJ io = GameObject.Instantiate<ItemOBJ>(itemObjListFromResources[id.name], new Vector3(id.position[0], id.position[1], id.position[2]), Quaternion.identity, l.transform);

                io.item = itemListFromResources[id.name];


                itemObjs.Add(io);
            }

            l.items = itemObjs;

            List<EnemyOBJ> enemyObjs = new List<EnemyOBJ>();


            foreach (EnemyOBJ enemyOBJ in l.enemies)
            {
                GameObject.Destroy(enemyOBJ.gameObject);
            }

            l.enemies = null;

            foreach (EnemyData ed in ld.enemies)
            {
                EnemyOBJ eo = GameObject.Instantiate<EnemyOBJ>(enemyObjListFromResources[ed.name.Split(' ')[0]], new Vector3(ed.position[0], ed.position[1], ed.position[2]), Quaternion.identity, l.transform);

                eo.name = ed.name;
                eo.maxHealth = ed.maxHealth;
                if (ed.weapon != string.Empty && ed.weapon != "")
                    eo.weapon = (Weapon)itemListFromResources[ed.weapon];
                if (ed.shield != string.Empty && ed.weapon != "")
                    eo.shield = (Shield)itemListFromResources[ed.shield];

                eo.sprite = characterSprites.GetSprite(ed.sprite);

                enemyObjs.Add(eo);
            }

            l.enemies = enemyObjs.ToArray();

            if (l.trader != null)
            {
                TraderData td = ld.trader;

                TraderOBJ to = GameObject.Instantiate<TraderOBJ>(traderObjListFromResources[td.name.Split(' ')[0]], new Vector3(td.position[0], td.position[1], td.position[2]), Quaternion.identity, l.transform);
                to.name = td.name;
                to.maxHealth = td.maxHealth;

                if (td.weapon != string.Empty && td.weapon != "")
                    to.weapon = (Weapon)itemListFromResources[td.weapon];

                to.sprite = characterSprites.GetSprite(td.sprite);

                to.trader.gold = td.gold;

                foreach (ItemData id in td.stock)
                {
                    to.stock.Add(itemListFromResources[id.name]);
                }

                l.trader = to;
            }
            l.makeLocation(false, true);

        }
        //Update Scene
        locationsMap.moveFromLoad(gameStateData.currentLocation);

        GameState.currentLocation = locationsMap.getLocation();

        //Update player UI
        uIManager.updatePlayerHealth(_player);
        
        uIManager.clearInventory();
       

        foreach (Item item in _player.getInventoryItems())
        {
            uIManager.addToPlayerInventory(item);
        }

        if (_player.weapon != null)
            uIManager.addToEquiped(_player.weapon);
        if (_player.shield != null)
            uIManager.addToEquiped(_player.shield);

        foreach (QuestItem qi in GameState.questItemsCollected)
        {
            uIManager.UpdateObjectiveText(qi);
        }

        uIManager.updateGold();
        uIManager.UpdateMinimap(GameState.currentLocation.name);
    }

    #endregion
}
