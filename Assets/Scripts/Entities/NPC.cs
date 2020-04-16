using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Entity
{
    private bool _isHostile = false;

    public bool isHostile { get => _isHostile; set => _isHostile = value; }

    public NPC()
    {

    }
    
}
