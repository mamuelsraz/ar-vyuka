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
        currentArObjectInstance = null;
    }

    public void CreateNewARObjectInstance(GameObject obj, ArObject arObj)
    {
        ArObjectInstance instance = new ArObjectInstance(obj, arObj);
        if (instance != currentArObjectInstance)
        {
            Debug.Log("new instance set");
            OnDeletedArObjInstance.Invoke();
            currentArObjectInstance = instance;
            OnNewArObjInstance.Invoke();
        }
    }
}


[SerializeField]
public enum AppState
{
    MenuState,
    ChooseState,
    PlaceState,
    ImagePlaceState,
    LookState
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
