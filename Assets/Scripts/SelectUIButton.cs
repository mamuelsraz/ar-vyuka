using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUIButton : MonoBehaviour
{
    public ArObject ARObject;

    public void Click()
    {
        AppManager.instance.AddState(1);
        AppManager.currentArObject = ARObject;
    }
}
