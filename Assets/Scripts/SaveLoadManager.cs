﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager
{
    private Dictionary<string, Item> itemListFromResources = new Dictionary<string, Item>();
    private Dictionary<string, ItemOBJ> itemObjListFromResources = new Dictionary<string, ItemOBJ>();
    private Dictionary<string, EnemyOBJ> enemyObjListFromResources = new Dictionary<string, EnemyOBJ>();

    private void getObjectsFromResources()
    {
        Item[] items = Resources.LoadAll<Item>("Items/");

        foreach (Item i in items)
        {
            itemListFromResources.Add(i.name, i);
        }

        ItemOBJ[] itemOBJs = Resources.LoadAll<ItemOBJ>("Items/Prefabs/");

        foreach (ItemOBJ i in itemOBJs)
        {
            itemObjListFromResources.Add(i.name, i);
        }

        EnemyOBJ[] enemyOBJs = Resources.LoadAll<EnemyOBJ>("Npcs/Enemies/");

        foreach (EnemyOBJ i in enemyOBJs)
        {
            enemyObjListFromResources.Add(i.name, i);
        }

    }

    [System.Serializable]
    public struct PlayerData
    {
        public int health;
        public int maxHealth;
        public string weapon;
        public string shield;
        public ItemData[] inventory;
        public string currentLocation;
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
    public struct ItemData
    {
        public string name;
        public float[] position;
    }


    //SAVE GAME TO JSON IN TXT FILE
    public void save(GameState gameState, LocationsMap locationsMap)
    {
        Player _player = gameState.player;

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

        playerData.currentLocation = locationsMap.getLocationName();

        //LOCATION DATA
        Locations locations = new Locations();
        Location[] locationsFromMap = locationsMap.getAllLocations();
        List<LocationData> lds = new List<LocationData>();
        foreach (Location l in locationsFromMap)
        {

            if (l.PlayerVisited)
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
                    ed.maxHealth = e._maxHealth;
                    if (e._weapon != null) ed.weapon = e._weapon.name;
                    if (e._shield != null) ed.shield = e._shield.name;
                    ed.sprite = e._sprite.name;

                    ed.position = new float[] { e.transform.position.x, e.transform.position.y, e.transform.position.z };

                    edList.Add(ed);
                }
                ld.enemies = edList.ToArray();
                lds.Add(ld);
            }

        }
        locations.locations = lds.ToArray();

        string JSonStringPlayer = JsonUtility.ToJson(playerData);
        string JsonStringLocations = JsonUtility.ToJson(locations);

        using (StreamWriter writer = new StreamWriter(@"SaveGame/Json.txt"))
        {
            writer.WriteLine(JSonStringPlayer);
            writer.WriteLine(JsonStringLocations);
        }
    }
    //LOAD GAME FROM SAVE FILE
    public void load(GameState gameState, LocationsMap locationsMap, UIManager uIManager)
    {
        getObjectsFromResources();
        string JSonStringPlayer = "";
        string JsonStringLocations = "";
        using (StreamReader reader = new StreamReader("SaveGame/Json.txt"))
        {
            JSonStringPlayer = reader.ReadLine();
            JsonStringLocations = reader.ReadLine();
        }

        PlayerData playerData = JsonUtility.FromJson<PlayerData>(JSonStringPlayer);
        Locations locations = JsonUtility.FromJson<Locations>(JsonStringLocations);
        //CREATE PLAYER FROM SAVE DATA
        Player _player = new Player();

        _player.health = playerData.health;
        _player.maxHealth = playerData.maxHealth;

        if (playerData.weapon != string.Empty && playerData.weapon != "")
            _player.weapon = (Weapon)itemListFromResources[playerData.weapon];
        if (playerData.weapon != string.Empty && playerData.shield != "")
            _player.shield = (Shield)itemListFromResources[playerData.shield];

        foreach (ItemData id in playerData.inventory)
        {
            _player.giveItem(itemListFromResources[id.name]);
        }

        gameState.player = _player;

        //EDIT EXISTING LOCATIONS WITH SAVE DATA

        foreach (LocationData ld in locations.locations)
        {
            Location l = locationsMap.getLocation(ld.locationName);

            l.PlayerVisited = true;

            List<ItemOBJ> itemObjs = new List<ItemOBJ>();

            foreach (ItemOBJ itemOBJ in l.Items)
            {
                Item i = null;
                l.takeItem(new string[] { itemOBJ.item.name },out i);
                GameObject.Destroy(itemOBJ.gameObject);
            }
            l.Items = null;


            foreach (ItemData id in ld.items)
            {
                ItemOBJ io = GameObject.Instantiate<ItemOBJ>(itemObjListFromResources[id.name], new Vector3(id.position[0], id.position[1], id.position[2]), Quaternion.identity);

                io.item = itemListFromResources[id.name];


                itemObjs.Add(io);
            }

            l.Items = itemObjs.ToArray();

            List<EnemyOBJ> enemyObjs = new List<EnemyOBJ>();


            foreach (EnemyOBJ enemyOBJ in l.Enemies)
            {
                GameObject.Destroy(enemyOBJ.gameObject);
            }

            l.Enemies = null;

            foreach (EnemyData ed in ld.enemies)
            {
                EnemyOBJ eo = GameObject.Instantiate<EnemyOBJ>(enemyObjListFromResources[ed.name], new Vector3(ed.position[0], ed.position[1], ed.position[2]), Quaternion.identity);

                eo.name = ed.name;
                eo._maxHealth = ed.maxHealth;
                if (ed.weapon != string.Empty && ed.weapon != "")
                    eo._weapon = (Weapon)itemListFromResources[ed.weapon];
                if (ed.shield != string.Empty && ed.weapon != "")
                    eo._shield = (Shield)itemListFromResources[ed.shield];

                eo._sprite = Resources.Load<Sprite>("/Sprites/" + ed.sprite);

                enemyObjs.Add(eo);
            }

            l.Enemies = enemyObjs.ToArray();

            l.makeLocation(false, true);

        }
        //Update Scene
        locationsMap.move(new string[] { playerData.currentLocation });

        gameState.currentLocation = locationsMap.getLocation();

        //Update player UI
        uIManager.updatePlayerHealth(_player);

        foreach (Item item in _player.getInventoryItems())
        {
            uIManager.addToPlayerInventory(item);
        }
        if (_player.weapon != null)
            uIManager.addToEquiped(_player.weapon);
        if (_player.shield != null)
            uIManager.addToEquiped(_player.shield);

        foreach (QuestItem qi in gameState.questItemsCollected)
        {
            uIManager.UpdateObjectiveText(gameState.questItemsCollected.IndexOf(qi));
        }
    }
}
