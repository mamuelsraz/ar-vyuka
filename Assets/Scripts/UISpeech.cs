using System.Collections;
using System.Collections.Generic;
using TextSpeech;
using UnityEngine;

public class UISpeech : MonoBehaviour
{
    public void PlaySound(string language)
    {
        TextToSpeech TTSManager = TextToSpeech.instance;
        TTSManager.Setting(language, TTSManager.pitch, TTSManager.rate);
        TTSManager.StartSpeak(language == "en-EN"? "Duck" : "Canard");
    }
}
