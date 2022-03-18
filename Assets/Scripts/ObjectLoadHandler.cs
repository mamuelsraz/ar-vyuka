using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class ObjectLoadHandler : MonoBehaviour
{
    public static Dictionary<Category, List<ArObject>> AllObjects = new Dictionary<Category, List<ArObject>>();
    public UnityEvent OnLoadedList;
    public UnityEvent OnLoadedArObj;
    public UnityEvent OnErrorArObj;
    public static ObjectLoadHandler instance;

    public List<GameObject> gameObjectsInMemory;

    //Musíme udelat novou branch!!
    string url = "https://mamuelsraz.github.io/ar-vyuka/Assets/Bundles/";
    string listName = "list";

    private void Awake()
    {
        gameObjectsInMemory = new List<GameObject>();
        if (instance == null) instance = this;
        else Destroy(this);

        LoadListFromURL();
    }

    public Dictionary<Category, List<ArObject>> LoadListFromResources()
    {
        ArObject[] loadedObjs = Resources.LoadAll<ArObject>("ARObjects");

        AllObjects = LoadList(loadedObjs);

        OnLoadedList?.Invoke();

        return AllObjects;
    }

    static Dictionary<Category, List<ArObject>> LoadList(ArObject[] loadedObjs)
    {
        Dictionary<Category, List<ArObject>> AllObjects = new Dictionary<Category, List<ArObject>>();

        foreach (var item in loadedObjs)
        {
            Debug.Log($"loaded: {item.NamesInLanguages[0].name} from {item.category}");

            if (!AllObjects.ContainsKey(item.category))
            {
                AllObjects.Add(item.category, new List<ArObject>());
            }

            List<ArObject> list = AllObjects[item.category];
            list.Add(item);
        }

        return AllObjects;
    }

    public void LoadListFromURL()
    {
        //LoadListFromResources();
        StartCoroutine(GetListRoutine());
    }

    public void LoadArObjFromUrl(ArObject arObject)
    {
        foreach (var obj in AppManager.instance.CachedInstances)
        {
            if (obj.ArObj.BundleName == arObject.BundleName)
            {
                OnLoadedArObj.Invoke();
                return;
            }
        }

        StartCoroutine(GetArObjRoutine(arObject.BundleName));
    }

    IEnumerator GetArObjRoutine(string bundleName)
    {
        string bundleURL = url + bundleName;

        Debug.Log("Requesting bundle at " + bundleURL);

        //request asset bundle
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(bundleURL);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("Network error");
            OnErrorArObj.Invoke();
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
            if (bundle != null)
            {
                GameObject loadedObject = bundle.LoadAllAssets<GameObject>()[0];

                if (loadedObject != null)
                {
                    gameObjectsInMemory.Add(loadedObject);
                    AppManager.instance.currentArObject.obj = loadedObject;

                    //bundle.Unload(false);

                    Debug.Log($"Assets for bundle {bundleName} successfully loaded");

                    OnLoadedArObj?.Invoke();
                }
                else
                {
                    Debug.Log("no assets of type ArObject in bundle");
                    OnErrorArObj.Invoke();
                }
            }
            else
            {
                Debug.Log("Not a valid asset bundle");
                OnErrorArObj.Invoke();
            }
        }
    }

    IEnumerator GetListRoutine()
    {

        string bundleURL = url + listName;

        Debug.Log("Requesting bundle at " + bundleURL);

        //request asset bundle
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(bundleURL);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("Network error");
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
            if (bundle != null)
            {
                ArObject[] loadedObjects = bundle.LoadAllAssets<ArObject>();
                if (loadedObjects.Length > 0)
                {
                    AllObjects = LoadList(loadedObjects);
                    bundle.Unload(false);

                    OnLoadedList?.Invoke();
                }
                else
                {
                    Debug.Log("no assets of type ArObject in bundle");
                }
            }
            else
            {
                Debug.Log("Not a valid asset bundle");
            }
        }
    }
}
