using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnStateEnterEvent : UnityEvent<AppState, AppState>{}

public class AppManager : MonoBehaviour
{
    public static AppManager instance;
    public static ArObjectInstance currentArObjectInstance;
    public static ArObject currentArObject;

    [HideInInspector]
    public OnStateEnterEvent OnStateEnter;
    [HideInInspector]
    public OnStateEnterEvent OnStateExit;

    private AppState currentState;
    public AppState CurrenState 
    {
        get { return currentState; }
        set {
            if (currentState != value)
            {
                OnStateExit.Invoke(currentState, value);
                OnStateEnter.Invoke(value, currentState);
                currentState = value;
            }
        }
    }

    private void Awake()
    {
        instance = this;
        CurrenState = AppState.MenuState;
    }

    public void ChangeState(int state)
    {
        CurrenState = (AppState)state;
    }
    public void AddState(int num)
    {
        CurrenState = (AppState)((int)currentState + num);
    }
}


[SerializeField]
public enum AppState
{
    MenuState,
    ChooseState,
    PlaceState,
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
