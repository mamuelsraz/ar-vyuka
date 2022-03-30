using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

[System.Serializable]
public class OnStateEnterEvent : UnityEvent<AppState, AppState> { }

public class AppManager : MonoBehaviour
{
    public static AppManager instance;

    public ArObjectInstance currentArObjectInstance
    {
        get;
        private set;
    }

    public List<ArObjectInstance> CachedInstances;

    public ArObject currentArObject;
    public ARSession session;
    public AppState[] ARSessionEnabled;

    [HideInInspector]
    public OnStateEnterEvent OnStateEnter;
    [HideInInspector]
    public OnStateEnterEvent OnStateExit;
    [HideInInspector]
    public UnityEvent OnNewArObjInstance;
    [HideInInspector]
    public UnityEvent OnDeletedArObjInstance;

    private AppState lastState;
    private AppState currentState;
    public AppState CurrenState
    {
        get { return currentState; }
        set
        {
            if (currentState != value)
            {
                lastState = currentState;
                OnStateExit.Invoke(currentState, value);
                OnStateEnter.Invoke(value, currentState);
                currentState = value;
            }
        }
    }

    private void Awake()
    {
        instance = this;
        CachedInstances = new List<ArObjectInstance>();
        CurrenState = AppState.MenuState;
        OnStateEnter.AddListener(ToggleARSession);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrenState == AppState.MenuState) Application.Quit();
            else
                CurrenState -= 1;
        }


    }

    public void ChangeState(string state)
    {
        CurrenState = (AppState)Enum.Parse(typeof(AppState), state);
    }

    void ToggleARSession(AppState appState, AppState lastState)
    {
        if (ARSessionEnabled.Contains(appState))
        {
            if (!session.isActiveAndEnabled) session.gameObject.SetActive(true);
        }
        else if (session.isActiveAndEnabled) session.gameObject.SetActive(false);
    }

    public void DestroyCurrentArObjInstance()
    {
        OnDeletedArObjInstance?.Invoke();

        if (currentArObjectInstance != null)
        {
            currentArObjectInstance.instance.transform.parent = null;
            currentArObjectInstance.instance.SetActive(false);
        }

        currentArObjectInstance = null;
    }

    public void DestroyInstance(ArObjectInstance instance)
    {
        currentArObjectInstance = instance;
        DestroyCurrentArObjInstance();
    }

    public void CreateNewARObjectInstance(ArObject arObj, Transform parent)
    {
        foreach (var item in CachedInstances)
        {
            if (item.ArObj == arObj)
            {
                SetupInstance(item, parent);

                Debug.Log("instance already cached: enabling it");

                DestroyCurrentArObjInstance();
                currentArObjectInstance = item;
                OnNewArObjInstance.Invoke();

                DebugPosition(parent);

                return;
            }
        }

        GameObject obj = Instantiate(arObj.obj, parent);
        ArObjectInstance instance = new ArObjectInstance(obj, arObj);

        SetupInstance(instance, parent);

        Debug.Log("new instance created");

        DestroyCurrentArObjInstance();
        currentArObjectInstance = instance;
        OnNewArObjInstance.Invoke();
        CachedInstances.Add(instance);

        DebugPosition(parent);
    }

    public void CreateNewARObjectInstanceNonDestroy(ArObject arObj, Transform parent)
    {

        foreach (var item in CachedInstances)
        {
            if (item.ArObj == arObj)
            {
                SetupInstance(item, parent);

                Debug.Log("instance already cached: enabling it");

                currentArObjectInstance = item;
                OnNewArObjInstance.Invoke();

                DebugPosition(parent);

                return;
            }
        }

        GameObject obj = Instantiate(arObj.obj, parent);
        ArObjectInstance instance = new ArObjectInstance(obj, arObj);

        SetupInstance(instance, parent);

        Debug.Log("new instance created");

        currentArObjectInstance = instance;
        OnNewArObjInstance.Invoke();
        CachedInstances.Add(instance);

        DebugPosition(parent);
    }

    void SetupInstance(ArObjectInstance ARinstance, Transform parent)
    {
        ARinstance.instance.transform.parent = parent;
        ARinstance.instance.transform.localPosition = Vector3.zero;
        ARinstance.instance.transform.localRotation = Quaternion.identity;
        ARinstance.instance.SetActive(true);

        Debug.LogWarning(ARinstance.instance.transform.position);
    }

    void DebugPosition(Transform parent)
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("Respawn"))
        {
            Destroy(item);
        }

        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obj.transform.parent = parent;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one * 0.1f;
        obj.tag = "Respawn";
        obj.name = Time.time.ToString();

        Debug.Log(obj.transform.position);
    }
}


[SerializeField]
public enum AppState
{
    MenuState,
    ChooseState,
    PlaceState,
    ImagePlaceState,
    LookState,
    TextLookState,
    QRChooseState
}

[SerializeField]
public class ArObjectInstance
{
    public GameObject instance;
    public ArObject ArObj;

    public ArObjectInstance(GameObject instance, ArObject ArObject)
    {
        this.instance = instance;
        this.ArObj = ArObject;
    }
}
