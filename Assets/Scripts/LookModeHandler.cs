using System.Collections;
using System.Collections.Generic;
using TextSpeech;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LookModeHandler : MonoBehaviour
{
    //https://github.com/j1mmyto9/Speech-And-Text-Unity-iOS-Android

    TextToSpeech TTSManager;

    public TextMeshProUGUI nameText;
    public Image loadingImage;
    [SerializeField] float rotationSpeed;

    bool loading;

    private void Start()
    {
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

    public void PlaySound(string language)
    {
        ArObjectInstance obj = AppManager.instance.currentArObjectInstance;

        if (AppManager.instance.currentArObject != null)
        {
            string name = NameInLanguage.Find(obj.ArObj.NamesInLanguages, language);

            if (name != null)
            {
                nameText.text = name;
                TTSManager.Setting(language, TTSManager.pitch, TTSManager.rate);
                TTSManager.StartSpeak(name);

                StartLoading();
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
        if (current == AppState.LookState)
        {
            AppManager.instance.DestroyCurrentArObjInstance();
            nameText.text = "";
        }
    }
}
