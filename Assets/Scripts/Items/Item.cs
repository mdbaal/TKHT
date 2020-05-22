using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    [SerializeField]
    private string _name = "";
    [TextArea()]
    [SerializeField]
    private string _description = "";
    [SerializeField]
    private Sprite _sprite = null;
    [SerializeField]
    private int _worth = 0;

    public string name { get => _name; set => _name = value; }
    public string description { get => _description; set => _description = value; }
    public Sprite sprite { get => _sprite; set => _sprite = value; }
    public int worth { get => _worth; set => _worth = value; }
}
