using System.Collections;
using System.Collections.Generic;
using TextSpeech;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(UIAppPanel))]
public class LookModeHandler : MonoBehaviour
{
    //https://github.com/j1mmyto9/Speech-And-Text-Unity-iOS-Android

    TextToSpeech TTSManager;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI spesPrepText;
    public TextMeshProUGUI unspesPrepText;

    public Image loadingImage;
    [SerializeField] float rotationSpeed;

    UIAppPanel uiPanel;
    bool loading;

    string currentLanguage = "en-EN";

    private void Start()
    {
        uiPanel = GetComponent<UIAppPanel>();

        TTSManager = TextToSpeech.instance;
        nameText.text = "";
        TextToSpeech.instance.onDoneEvent.AddListener(StopLoading);
        AppManager.instance.OnStateExit.AddListener(ExitLookMode);

        TTSManager.rate = 1;
        TTSManager.pitch = 1;
    }

    private void Update()
    {
        if (loading)
        {
            loadingImage.transform.eulerAngles = new Vector3(0, 0, loadingImage.transform.eulerAngles.z + rotationSpeed * Time.deltaTime);
        }
    }

    public void PlaySound(int mode)
    {
        ArObjectInstance obj = AppManager.instance.currentArObjectInstance;

        if (AppManager.instance.currentArObject != null)
        {
            NameInLanguage text = obj.ArObj.Find(currentLanguage);

            if (text != null)
            {
                string speech = "";

                switch (mode)
                {
                    default:
                        break;
                    case 0:
                        speech = text.name;
                        break;
                    case 1:
                        speech = text.unspesPrep + " " + text.name;
                        break;
                    case 2:
                        speech = text.spesPrep + " " + text.name;
                        break;
                }

                TTSManager.Setting(currentLanguage, TTSManager.pitch, TTSManager.rate);
                TTSManager.StartSpeak(speech);

                StartLoading();
            }
        }
        else
        {
            Debug.LogError("No current AR object to play");
        }
    }

    public void ChangeLanguage(string language)
    {
        currentLanguage = language;

        ArObjectInstance obj = AppManager.instance.currentArObjectInstance;

        if (AppManager.instance.currentArObject != null)
        {
            NameInLanguage text = obj.ArObj.Find(currentLanguage);

            if (name != null)
            {
                nameText.text = text.name;
                unspesPrepText.text = text.unspesPrep;
                spesPrepText.text = text.spesPrep;
            }
        }
        else
        {
            Debug.LogError("No current AR object to play");
        }
    }


    void StartLoading()
    {
        loadingImage.DOFade(0.5f, UIAppPanel.UISpeed);
        loading = true;
    }

    void StopLoading()
    {
        loadingImage.DOFade(0.0f, UIAppPanel.UISpeed);
        loading = false;
    }

    void ExitLookMode(AppState current, AppState last)
    {
        if (current == uiPanel.targetState)
        {
            AppManager.instance.DestroyCurrentArObjInstance();
            nameText.text = "";
        }
    }
}
