using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationsMap : MonoBehaviour
{
    private Dictionary<string, Location> locations = new Dictionary<string, Location>();

    [SerializeField]
    private Location currentLocation = null;

    private void Start()
    {
        getAllLocationsFromScene();
    }

    public int move(string[] newloc)
    {
        string _newloc = "";
        foreach(string s in newloc)
        {
            _newloc += s + " ";
        }
        _newloc = _newloc.Trim();
        if (locations.ContainsKey(_newloc))
        {
            if (currentLocation.name == _newloc) return -2;

            if (currentLocation.hasNeighbour(_newloc))
            {
                Location old = currentLocation;
                old.leave();
                currentLocation = locations[_newloc];
                currentLocation.enter();
                currentLocation.PlayerVisited = true;
                
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

    public string getLocationName()
    {
        return currentLocation.name;
    }


    

    public string getLocationDescription()
    {
        string outstring = "You're at " + getLocationDescriptionShort() + "\n";

        string items  = currentLocation.listItems();
        string npcs = currentLocation.getNpcs();
        if (items.Equals(string.Empty)){
            outstring += "  When you look around you don't really see anything interresting";
        }
        else
        {
            outstring += "When you look around you find these items\n";
            outstring += items;

            outstring += "And these people\n";
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

    private void getAllLocationsFromScene()
    {
        Location[] _locations = this.GetComponentsInChildren<Location>();
        
        foreach(Location l in _locations)
        {
            l.makeLocation(true,false);
            locations.Add(l.name, l);
        }

        currentLocation = _locations[0];
        currentLocation.enter();
        currentLocation.PlayerVisited = true;
    }

    public Location[] getAllLocations()
    {
        return this.GetComponentsInChildren<Location>();
    }

}
