using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AR object", menuName = "AR/ArObject", order = 1)]
public class ArObject : ScriptableObject
{
    public GameObject obj;
    public Category category;
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

[SerializeField]
public enum Category 
{
    food,
    animals,
    other
}
