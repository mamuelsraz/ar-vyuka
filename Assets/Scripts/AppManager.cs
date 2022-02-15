using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnStateEnterEvent : UnityEvent<AppState>{}

public class AppManager : MonoBehaviour
{
    public static AppManager instance;
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
                OnStateExit.Invoke(currentState);
                currentState = value;
                OnStateEnter.Invoke(currentState);
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
}


[SerializeField]
public enum AppState
{
    MenuState,
    PlaceState,
    LookState
}
