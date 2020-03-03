using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationsMap : MonoBehaviour
{
    private Dictionary<string, Location> locations = new Dictionary<string, Location>();

    public OutputManager outputManager;
    [SerializeField]
    private Location currentLocation = null;

    private void Awake()
    {
        getAllLocations();
    }

    public void move(string[] newloc)
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
                currentLocation = locations[_newloc];
                outputManager.outputMessage("You went to " + _newloc);
            }
            else
            {
                outputManager.outputMessage("Can't go there from here");
            }
        }
        else
        {
            outputManager.outputMessage("This place doesn't exist");
        }
    }

    public string getLocationName()
    {
        return currentLocation.name;
    }

    public string getLocationDescription()
    {
        return "You're at " + currentLocation.name + ". " + currentLocation.getDescription()+ ". " + "When you look around you see " + currentLocation.listItems() + "."; 
    }

    public Location GetLocation()
    {
        return currentLocation;
    }

    private void getAllLocations()
    {
        Location[] _locations = this.GetComponentsInChildren<Location>();
        currentLocation = _locations[0];
        foreach(Location l in _locations)
        {
            locations.Add(l.name, l);
        }
    }

}
