using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationsMap : MonoBehaviour
{
    private Dictionary<string, Location> locations = new Dictionary<string, Location>();

    [SerializeField]
    private Location currentLocation = null;

    private bool _allLocationsLoaded = false;

    public bool allLocationsMade { get => _allLocationsLoaded; set => _allLocationsLoaded = value; }

    //Move to new location
    public int move(string[] newloc)
    {
        string _newloc = "";
        foreach (string s in newloc)
        {
            _newloc += s + " ";
        }
        _newloc = _newloc.Trim();
        if (locations.ContainsKey(_newloc))
        {
            if (currentLocation.name == _newloc) return -2;
            if (currentLocation.allEnemiesDeadToContinue && !currentLocation.allEnemiesDead)
            {
                return -3;
            }
            else if (currentLocation.hasNeighbour(_newloc))
            {
                Location old = currentLocation;
                old.leave();
                currentLocation = locations[_newloc];
                currentLocation.enter();
                currentLocation.playerVisited = true;

                return 1;
            }
            else
            {
                return -1;
            }
        }
        else
        {
            return 0;
        }
    }

    //Move to new location
    public void moveFromLoad(string newloc)
    {
        if (locations.ContainsKey(newloc))
        {
            Location old = currentLocation;
            old.leave();
            currentLocation = locations[newloc];
            currentLocation.enter();
            currentLocation.playerVisited = true;
        }
    }
    //Get name of current location
    public string getLocationName()
    {
        return currentLocation.name;
    }

    //Get location description called with look
    public string getLocationDescription()
    {
        string outstring = "You're at " + getLocationDescriptionShort() + "\n";

        string items = currentLocation.listItems();
        string npcs = currentLocation.getNpcs();

        if (items.Equals(string.Empty))
        {
            outstring += "  When you look around you don't really see anything interesting\n";


        }
        else
        {
            outstring += "  When you look around you find these items\n";
            outstring += items;
        }
        if (!npcs.Equals(string.Empty))
        {
            outstring += "  There are some people about\n";
            outstring += npcs;
        }

        return outstring;
    }

    public string getLocationDescriptionShort()
    {
        return currentLocation.getDescription();
    }

    public Location getLocation()
    {
        return currentLocation;
    }

    public Location getLocation(string locName)
    {
        foreach (Location l in locations.Values)
        {
            if (l.name == locName) return l;
        }
        return null;
    }
    //Look for all locations in scene and make them
    public void makeLocations()
    {
        Location[] _locations = this.GetComponentsInChildren<Location>();

        foreach (Location l in _locations)
        {
            l.makeLocation(true, false);
            locations.Add(l.name, l);
        }

        currentLocation = _locations[0];
        currentLocation.enter();
        currentLocation.playerVisited = true;

        allLocationsMade = true;
    }

    public Location[] getAllLocations()
    {
        return this.GetComponentsInChildren<Location>();
    }

}
