using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CreateCustomObjects
{
    [MenuItem("GameObject/UI/Custom InputField", false, 10)]
    static void CreateCustomInputField(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject input = new GameObject("Custom InputField");
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(input, GameObject.FindGameObjectWithTag("UI") as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(input, "Create " + input.name);
        Image img = input.AddComponent<Image>();
        CInputField _input = input.AddComponent<CInputField>();
        

        _input.targetGraphic = img;

        GameObject text = new GameObject("Text");

        Text _text = text.AddComponent<Text>();

        _input.textComponent = _text;

        _text.supportRichText = false;
        _text.color = new Color(0, 0, 0);
        
        GameObjectUtility.SetParentAndAlign(text, input as GameObject);


        Selection.activeObject = input;
    }
}
