using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class TabButton : MonoBehaviour
{
    public bool startSelected;
    public string tabGroup;
    public GameObject tab;
    public static Dictionary<string, List<TabButton>> tabs;

    Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Select);

        Subscribe();

        if (startSelected) Select();
        else Deselect();
    }

    void Subscribe()
    {
        if (tabs == null) tabs = new Dictionary<string, List<TabButton>>();

        if (!tabs.ContainsKey(tabGroup))
        {
            tabs.Add(tabGroup, new List<TabButton>());
        }

        tabs[tabGroup].Add(this);
    }

    public void Select()
    {
        foreach (var button in tabs[tabGroup])
        {
            button.Deselect();
        }

        tab.SetActive(true);
    }

    public void Deselect()
    {
        tab.SetActive(false);
    }
}
