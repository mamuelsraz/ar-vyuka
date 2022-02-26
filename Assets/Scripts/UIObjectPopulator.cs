using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class UIObjectPopulator : MonoBehaviour
{
    public ObjectLoadHandler loader;
    public Transform panelParent;
    public GameObject prefab;
    public GameObject categoryPrefab;
    public Transform categoryHolder;
    [Space]
    Category selectedCategory = 0;

    private void Start()
    {
        InitCategories();
        //Populate();
    }

    void InitCategories()
    {
        foreach (var category in loader.AllObjects)
        {
            Button item = Instantiate(categoryPrefab, categoryHolder).GetComponent<Button>();

            item.GetComponentInChildren<TextMeshProUGUI>().text = category.Key.ToString();
            SelectUIButton uiButton = item.gameObject.AddComponent<SelectUIButton>();
            uiButton.category = category.Key;
            uiButton.ui = this;
            item.onClick.AddListener(uiButton.ClickCategory);
        }
    }

    void Populate()
    {
        var category = loader.AllObjects[selectedCategory];
        foreach (var item in category)
        {
            CreateNewElement(item);
        }
    }

    void Delete()
    {
        foreach (Transform child in panelParent)
        {
            Destroy(child.gameObject);
        }
    }

    void CreateNewElement(ArObject ARObj)
    {
        GameObject instance = Instantiate(prefab, panelParent);
        instance.GetComponentInChildren<TextMeshProUGUI>().text = ARObj.name;

        SelectUIButton button = instance.AddComponent<SelectUIButton>();
        button.ARObject = ARObj;
        instance.GetComponentInChildren<Button>().onClick.AddListener(button.ClickArObj);
    }

    public void ChangeCategory(Category category)
    {
        selectedCategory = category;
        Delete();
        Populate();
    }
}
