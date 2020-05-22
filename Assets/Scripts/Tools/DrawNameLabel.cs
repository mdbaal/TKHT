using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawNameLabel : MonoBehaviour
{
    [Header("In pixels")]
    [SerializeField]
    private float heightOffset;
    [Header("In percentage")]
    [Range(.1f, 1f)]
    [SerializeField]
    private float widthOffset = .1f;
    [SerializeField]
    private int fontSize;
    [SerializeField]
    private int padding;
    [SerializeField]
    private Texture2D labelTexture;
    [SerializeField]
    private bool wordWrap = false;

    [SerializeField]
    private Color textColor;
    [Range(0,1)]
    [SerializeField]
    private float alpha = 1;

    public GameState gameState;
    

    private void OnGUI()
    {
     
        if (!gameState.finishedTutorial || gameState.player.health <= 0 || gameState.allQuestItemsCollected) return;

        float objectCenter = this.GetComponent<SpriteRenderer>().bounds.center.x;

        textColor.a = alpha;

        Vector2 playerMiddle = new Vector2(objectCenter, this.transform.position.y);
        Vector2 pos = Camera.main.WorldToScreenPoint(playerMiddle);

        GUIStyle guiStyle = new GUIStyle();
        guiStyle.normal.background = labelTexture;
        guiStyle.fontSize = fontSize;
        guiStyle.fontStyle = FontStyle.Bold;
        guiStyle.padding = new RectOffset(padding, padding, padding, padding);
        guiStyle.wordWrap = wordWrap;
        guiStyle.normal.textColor = textColor;

        Vector2 textSize = guiStyle.CalcSize(new GUIContent(this.name));

        Rect rect = new Rect(pos.x, Screen.height - pos.y, textSize.x, textSize.y);

        rect.y -= heightOffset;
        if (this.GetComponent<SpriteRenderer>().flipX) 
            rect.x -= rect.width * (widthOffset + widthOffset);
        else
            rect.x -= rect.width * widthOffset;

        GUI.Box(rect, this.name, guiStyle);

    }
}
