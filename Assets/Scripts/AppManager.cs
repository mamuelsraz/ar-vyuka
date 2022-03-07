using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

[System.Serializable]
public class OnStateEnterEvent : UnityEvent<AppState, AppState> { }

public class AppManager : MonoBehaviour
{
    public static AppManager instance;
    public static ArObjectInstance currentArObjectInstance;
    public static ArObject currentArObject;
    public ARSession session;
    public AppState[] ARSessionEnabled;

    [HideInInspector]
    public OnStateEnterEvent OnStateEnter;
    [HideInInspector]
    public OnStateEnterEvent OnStateExit;

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

    void ScreenDimensionsChanged()
    {
        Debug.Log("ScreenDimensionsChanged");
    }

    private void Awake()
    {
        instance = this;
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
