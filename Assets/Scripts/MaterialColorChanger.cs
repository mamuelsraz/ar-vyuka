using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialColorChanger : MonoBehaviour
{
    public string tagName;
    public List<MeshRenderer> affectedMeshes;
    public List<Material> defaultMaterials;

    private void Start()
    {
        AppManager.instance.OnNewArObjInstance.AddListener(NewArObjInstance);
    }


    public void NewArObjInstance()
    {
        Debug.Log("searching for materials");

        affectedMeshes = new List<MeshRenderer>();
        defaultMaterials = new List<Material>();

        foreach (var item in AppManager.instance.currentArObjectInstance.instance.GetComponentsInChildren<MeshRenderer>())
        {
            if (item.tag == tagName)
            {
                affectedMeshes.Add(item);
                defaultMaterials.Add(item.material);
            }
        }
    }

    public void ChangeCol(Material mat)
    {
        if (affectedMeshes != null)
        {
            foreach (var item in affectedMeshes)
            {
                item.material = mat;
            }
        }
    }

    public void ResetCol()
    {
        if (affectedMeshes != null)
        {
            for (int i = 0; i < affectedMeshes.Count; i++)
            {
                affectedMeshes[i].material = defaultMaterials[i];
            }
        }
    }
}
