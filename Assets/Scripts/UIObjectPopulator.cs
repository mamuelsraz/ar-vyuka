using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class UIObjectPopulator : MonoBehaviour
{
    public Transform panelParent;
    public Transform categoryParent;
    [Space]
    public GameObject prefab;
    public GameObject categoryButtonPrefab;
    public GameObject categoryPanelPrefab;

    private void Start()
    {

    }

    public void InitCategories()
    {
        bool first = true;

        foreach (var category in ObjectLoadHandler.AllObjects)
        {
            //category button
            Button button = Instantiate(categoryButtonPrefab, categoryParent).GetComponent<Button>();

            GameObject panel = Instantiate(categoryPanelPrefab, panelParent);

            button.GetComponentInChildren<TextMeshProUGUI>().text = category.Key.ToString();
            TabButton tabButton = button.gameObject.GetComponent<TabButton>();
            tabButton.tabGroup = "category";
            tabButton.tab = panel;

            if (first) { first = false; tabButton.startSelected = true; }

            //category panel
            foreach (var item in category.Value)
            {
                CreateNewElement(item, panel.transform);
            }
        }
    }

    void CreateNewElement(ArObject ARObj, Transform parent)
    {
        GameObject instance = Instantiate(prefab, parent);
        instance.GetComponentInChildren<TextMeshProUGUI>().text = ARObj.name;

        SelectUIButton button = instance.AddComponent<SelectUIButton>();
        button.ARObject = ARObj;
        //not the right way to do it
        instance.transform.Find("Play").GetComponent<Button>().onClick.AddListener(button.ClickArObj);
        instance.transform.Find("QRCode").GetComponent<Button>().onClick.AddListener(button.ClickQRObj);
    }
}
