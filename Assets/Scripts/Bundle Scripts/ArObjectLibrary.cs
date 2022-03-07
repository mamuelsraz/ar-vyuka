using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ar object library", menuName = "AR/ArObjectLibrary", order = 1)]
public class ArObjectLibrary : ScriptableObject
{
    public List<Category> keys;
    public List<PaneBoze> values;

    //Ano ja vím jak to vypada ale je to jediny zpusob jak prijmout unitko aby tuhle obludu serializovalo
    public void Serialize(Dictionary<Category, List<ArObject>> lib)
    {
        keys = new List<Category>();
        values = new List<PaneBoze>();

        foreach (var key in lib)
        {
            keys.Add(key.Key);
            PaneBoze kurvaProc = new PaneBoze();
            kurvaProc.objs = key.Value;
            values.Add(kurvaProc);
        }
    }

    public Dictionary<Category, List<ArObject>> Deserialize()
    {
        Dictionary<Category, List<ArObject>> dictionary = new Dictionary<Category, List<ArObject>>();

        for (int i = 0; i < keys.Count; i++)
        {
            dictionary.Add(keys[i], values[i].objs);
        }

        return dictionary;
    }
}

//Ano ja vím jak to vypadá
[System.Serializable]
public class PaneBoze {
    public List<ArObject> objs;
}
