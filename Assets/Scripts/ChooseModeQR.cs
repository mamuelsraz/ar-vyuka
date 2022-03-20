using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChooseModeQR : MonoBehaviour
{
    public GameObject toggleMenu;
    public TextMeshProUGUI text;

    List<ArObject> selected;

    private void Start()
    {
        text.text = "";
    }

    public void Subscribe(ArObject obj)
    {
        if(selected == null) selected = new List<ArObject>();

        selected.Add(obj);
        UpdateText();
    }

    public void UnSubscribe(ArObject obj)
    {
        if (selected == null) selected = new List<ArObject>();

        selected.Remove(obj);
        UpdateText();
    }

    public void Enter()
    {

    }

    void UpdateText()
    {
        if (selected.Count == 0)
        {
            toggleMenu.SetActive(false);
            text.text = "";
            return;
        }

        toggleMenu.SetActive(true);

        string str = "použité objekty:";
        foreach (var item in selected)
        {
            str += " ";
            str += item.name;
        }
        text.text = str;
    }
}
