using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextLookModeHandler : MonoBehaviour
{
    public ArObject targetArObj;
    public TMP_InputField inputField;
    public MaterialColorChanger colorChanger;
    SimpleHelvetica text;

    private void Start()
    {
        AppManager.instance.OnStateEnter.AddListener(TryInit);
        inputField.onEndEdit.AddListener(TextChanged);
    }

    void Init()
    {
        if (AppManager.instance.currentArObject == targetArObj)
        {
            text = AppManager.instance.currentArObjectInstance.instance.GetComponentInChildren<SimpleHelvetica>();
            text.Text = inputField.text;
            text.GenerateText();
            colorChanger.NewArObjInstance();
        }
    }

    void TryInit(AppState current, AppState old)
    {
        if (current == AppState.TextLookState)
        {
            Init();
        }
    }

    public void TextChanged(string txt)
    {
        ArObject obj = AppManager.instance.currentArObject;
        foreach (var str in obj.NamesInLanguages)
        {
            str.name = inputField.text;
        }

        if (text == null) return;

        text.Text = inputField.text;
        text.GenerateText();
        colorChanger.NewArObjInstance();
    }
}
