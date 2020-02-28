using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeepInputActive : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField inputField;
    void Start()
    {
        StartCoroutine(activeCheck());
    }

    IEnumerator activeCheck()
    {
        yield return new WaitUntil(() => !inputField.isFocused);
        inputField.ActivateInputField();
        inputField.text = "> ";
        inputField.caretPosition = 2;
        StartCoroutine(activeCheck());
    }
}
