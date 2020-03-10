using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationsMap : MonoBehaviour
{
    private Dictionary<string, Location> locations = new Dictionary<string, Location>();

    [SerializeField]
    private Location currentLocation = null;

    private void Awake()
    {
        getAllLocations();
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
            if (currentLocation.hasNeighbour(_newloc))
            {
                Location old = currentLocation;
                old.leave();
                currentLocation = locations[_newloc];
                currentLocation.enter();
                
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
        return "You're at " + currentLocation.name + ".\n  " + currentLocation.getDescription()+ ". \n" + "  When you look around you see: \n" + currentLocation.listItems(); 
    }

    public Location GetLocation()
    {
        return currentLocation;
    }

    private void getAllLocations()
    {
        Location[] _locations = this.GetComponentsInChildren<Location>();
        currentLocation = _locations[0];
        currentLocation.enter();
        foreach(Location l in _locations)
        {
            locations.Add(l.name, l);
        }
    }

}
