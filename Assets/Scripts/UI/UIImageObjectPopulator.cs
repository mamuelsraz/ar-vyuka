using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIImageObjectPopulator : MonoBehaviour
{
    public int maxDownloadCount = 5;
    public Transform panelParent;
    public Transform categoryParent;
    [Space]
    public GameObject prefab;
    public GameObject categoryButtonPrefab;
    public GameObject categoryPanelPrefab;
    [Space]
    public TextMeshProUGUI selectedText;
    public Sprite selectedSprite;
    public Sprite deselectedSprite;

    List<ArObject> arObjects;
    List<Image> buttons;
    [HideInInspector]
    public List<ArObject> selected;

    int toDownload = 0;
    bool downloading;

    private void Start()
    {
        ObjectLoadHandler.instance.OnLoadedArObj.AddListener(OnLoaded);
    }

    public void InitCategories()
    {
        arObjects = new List<ArObject>();
        selected = new List<ArObject>();
        buttons = new List<Image>();

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

        arObjects.Add(ARObj);

        Button b = instance.GetComponentInChildren<Button>();
        Transform child = b.transform.GetChild(0);
        buttons.Add(child.GetComponent<Image>());

        int i = arObjects.Count - 1;
        b.onClick.AddListener(delegate { ButtonClicked(i); });
    }

    void ButtonClicked(int num)
    {
        if (selected.Contains(arObjects[num]))
        {
            selected.Remove(arObjects[num]);
        }
        else
        {
            if (selected.Count <= maxDownloadCount)
            {
                selected.Add(arObjects[num]);
            }
        }

        UpdateButton(num);
        selectedText.text = $"selected: {selected.Count}/{maxDownloadCount}";
    }

    public void LoadString()
    {
        selected = new List<ArObject>();

        string clipBoard = GUIUtility.systemCopyBuffer;

        string[] splitArray = clipBoard.Split(char.Parse("+"));
        foreach (var candidName in splitArray)
        {

            foreach (var arObj in arObjects)
            {
                if (candidName == arObj.name)
                {
                    selected.Add(arObj);
                }

            }
        }

        for (int i = 0; i < buttons.Count; i++)
        {
            UpdateButton(i);
        }
        selectedText.text = $"selected: {selected.Count}/5";
    }

    void UpdateButton(int index)
    {
        if (selected.Contains(arObjects[index]))
        {
            buttons[index].sprite = selectedSprite;
        }
        else buttons[index].sprite = deselectedSprite;
    }

    public void ExportString()
    {
        string s = "";

        foreach (var item in selected)
        {
            s += item.name;
            s += "+";
        }

        TextEditor te = new TextEditor();
        te.text = s;
        te.SelectAll();
        te.Copy();
    }

    void OnLoaded()
    {
        if (!downloading) return;

        toDownload -= 1;

        selectedText.text = $"downloaded {selected.Count - toDownload}/{selected.Count}";

        if (toDownload == 0)
        {
            downloading = false;

            AppManager.instance.CurrenState = AppState.ImagePlaceState;
        }
    }

    public void Execute()
    {
        if (selected.Count == 0) return;

        downloading = true;
        toDownload = selected.Count;

        selectedText.text = "downloaded 0/" + selected.Count;

        foreach (var arObj in selected)
        {
            ObjectLoadHandler.instance.LoadArObjFromUrl(arObj);
        }
    }
}
