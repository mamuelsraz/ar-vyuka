using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectLoadHandler : MonoBehaviour
{
    public static Dictionary<Category, List<ArObject>> AllObjects = new Dictionary<Category, List<ArObject>>();

    static string url = "";

    private void Awake()
    {
        //download from web next
        LoadFromResources();
    }

    public static Dictionary<Category, List<ArObject>> LoadFromResources()
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

        return AllObjects;
    }

    public static Dictionary<Category, List<ArObject>> LoadFromURL()
    {
        return LoadFromURL(url);
    }

    public static Dictionary<Category, List<ArObject>> LoadFromURL(string url)
    {
        return null;
    }
}
