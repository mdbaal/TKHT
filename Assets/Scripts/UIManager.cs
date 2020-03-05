using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Output scrollrect")]
    public ScrollRect textScroll;

    public void toBottom()
    {
        Canvas.ForceUpdateCanvases();

        textScroll.verticalNormalizedPosition = 0;
    }
}
