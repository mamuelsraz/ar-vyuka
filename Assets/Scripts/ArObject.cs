using UnityEngine;

[CreateAssetMenu(fileName = "New AR object", menuName = "AR/ArObject", order = 1)]
public class ArObject : ScriptableObject
{
    public GameObject obj;
    public string enName;
    public string frName;

    public GameObject Instantiate(Vector3 position)
    {
        return Instantiate(position, Quaternion.identity);
    }

    public GameObject Instantiate(Vector3 position, Quaternion rotation)
    {
        GameObject newObject = Instantiate(obj, position, rotation);
        ArObjectHolder holder = newObject.AddComponent<ArObjectHolder>();
        holder.obj = this;
        return newObject;
    }

    public GameObject Instantiate(Transform parent)
    {
        GameObject newObject = Instantiate(obj, parent);
        ArObjectHolder holder = obj.AddComponent<ArObjectHolder>();
        holder.obj = this;
        return newObject;
    }
}
