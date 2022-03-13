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
    [Header("Effects")]
    public TabButtonEffect effect;
    public Image image;

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

        PlayEffect(true);
        tab.SetActive(true);
    }

    public void Deselect()
    {
        PlayEffect(false);
        tab.SetActive(false);
    }

    void PlayEffect(bool open)
    {
        switch (effect)
        {
            case TabButtonEffect.None:
                break;
            case TabButtonEffect.ShadowButton:
                if (open)
                {
                    image.color = new Color(0.9f, 0.9f, 0.9f);
                }
                else
                {
                    image.color = new Color(0, 0, 0, 0);
                }
                break;
            default:
                break;
        }
    }
}

[SerializeField]
public enum TabButtonEffect
{
    None,
    ShadowButton
}
