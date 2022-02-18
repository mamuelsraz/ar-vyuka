using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIObjectPopulator : MonoBehaviour
{
    public ObjectLoadHandler loader;
    public Transform panelParent;
    public GameObject prefab;

    private void Start()
    {
        Populate();
    }

    void Populate()
    {
        foreach (var category in loader.AllObjects)
        {
            foreach (var item in category.Value)
            {
                CreateNewElement(item);
            }
        }
    }

    void CreateNewElement(ArObject ARObj)
    {
        GameObject instance = Instantiate(prefab, panelParent);
        instance.GetComponentInChildren<TextMeshProUGUI>().text = ARObj.name;

        SelectUIButton button = instance.AddComponent<SelectUIButton>();
        button.ARObject = ARObj;
        instance.GetComponentInChildren<Button>().onClick.AddListener(button.Click);
    }
}
