using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookModeSliders : MonoBehaviour
{
    public Slider scaleSlider;
    public Slider rotationSlider;

    private void Start()
    {
        scaleSlider.onValueChanged.AddListener(OnScaleChanged);
        rotationSlider.onValueChanged.AddListener(OnRotationChanged);
    }

    void OnScaleChanged(float value)
    {
        AppManager.instance.currentArObjectInstance.instance.transform.localScale = Vector3.one * value;
    }

    void OnRotationChanged(float value)
    {
        AppManager.instance.currentArObjectInstance.instance.transform.localEulerAngles = new Vector3(0, value, 0);
    }
}
