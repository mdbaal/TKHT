using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public new string name = "";
    [TextArea()]
    public string description = "";

    public Sprite sprite = null;

    public int worth = 0;
}
