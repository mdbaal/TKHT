using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawNameLabel : MonoBehaviour
{
    [Header("In pixels")]
    public float heightOffset;
    [Header("In percentage")]
    [Range(.1f, 1f)]
    public float widthOffset = .1f;

    public int fontSize;
    public int padding;

    public Texture2D labelTexture;

    public bool wordWrap = false;


    public Color textColor;
    [Range(0,1)]
    public float alpha = 1;
    GameState gameState = null;

    private void OnGUI()
    {
        if (gameState == null) gameState = FindObjectOfType<GameState>();
        if (gameState == null) return;
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
