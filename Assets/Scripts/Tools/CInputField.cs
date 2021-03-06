﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CInputField : InputField
{
    protected override void Awake()
    {
        StartCoroutine(activeCheck());
        this.onValueChanged.AddListener(delegate {
            if(this.text.Length < 2)
            {
                this.text = "> ";
                this.caretPosition = this.text.Length;
            }
        });

        
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        this.DeactivateInputField();
        StartCoroutine(activeCheck());
    }

     public override void OnSelect(BaseEventData eventData)
     {
         base.OnSelect(eventData);

         StartCoroutine(activeCheck());
    }


    public override void OnSubmit(BaseEventData eventData)
    {
        
    }
    

    IEnumerator activeCheck()
    {
        yield return new WaitUntil(() => !this.isFocused);
        yield return new WaitForEndOfFrame();
        this.Select();
        this.ActivateInputField();
        this.text = "> ";
        this.caretPosition = this.text.Length;
        StartCoroutine(activeCheck());
    }


}
