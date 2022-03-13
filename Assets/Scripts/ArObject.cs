using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New AR object", menuName = "AR/ArObject", order = 1)]
public class ArObject : ScriptableObject
{
    public GameObject obj;
    public Category category;
    public string BundleName;
    [Space(20)]
    public NameInLanguage[] NamesInLanguages;
}


[System.Serializable]
public class NameInLanguage
{
    public string name;
    public string language;

    public static string Find(NameInLanguage[] names, string key)
    {
        foreach (var item in names)
        {
            if (item.language == key) return item.name;
        }

        Debug.LogWarning("No names found by language");
        return null;
    }
}

[System.Serializable]
[SerializeField]
public enum Category
{
    food,
    animals,
    other
}
