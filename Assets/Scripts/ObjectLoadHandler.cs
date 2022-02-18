using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectLoadHandler : MonoBehaviour
{
    public Dictionary<Category, List<ArObject>> AllObjects = new Dictionary<Category, List<ArObject>>();

    public UnityEvent OnLoaded;

    private void Awake()
    {
        LoadFromResources();
    }

    void LoadFromResources()
    {
        ArObject[] loadedObjs = Resources.LoadAll<ArObject>("ARObjects");

        foreach (var item in loadedObjs)
        {
            Debug.Log($"loaded: {item.category.ToString()} {item.NamesInLanguages[0].name}");

            if (!AllObjects.ContainsKey(item.category))
            {
                AllObjects.Add(item.category, new List<ArObject>());
            }

            List<ArObject>  list = AllObjects[item.category];
            list.Add(item);
        }

        OnLoaded.Invoke();
    }
}
