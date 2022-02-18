using System.Collections;
using System.Collections.Generic;
using TextSpeech;
using TMPro;
using UnityEngine;

public class LookModeHandler : MonoBehaviour
{
    //https://github.com/j1mmyto9/Speech-And-Text-Unity-iOS-Android

    TextToSpeech TTSManager;

    public TextMeshProUGUI nameText;

    private void Start()
    {
        TTSManager = TextToSpeech.instance;
        nameText.text = "";
    }
    public void PlaySound(string language)
    {
        ArObjectInstance obj = AppManager.currentArObjectInstance;

        if (AppManager.currentArObject != null)
        {
            string name = NameInLanguage.Find(obj.ArObj.NamesInLanguages, language);

            if (name != null)
            {
                nameText.text = name;
                TTSManager.Setting(language, TTSManager.pitch, TTSManager.rate);

                TTSManager.StartSpeak(name);
            }
        }
        else
        {
            Debug.LogError("No current AR object to play");
        }
    }
}
