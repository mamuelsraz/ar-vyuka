using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ImagePicker : MonoBehaviour
{
    public string name;
    public TMP_InputField inputField;
    [Space]
    public ArObject arObject;
    public Material material;
    public RawImage image;
    public float idealScale = 0.2f;
    Vector2 dimensions = new Vector2();

    private void Start()
    {
        AppManager.instance.OnNewArObjInstance.AddListener(NewArObjInstance);

        LoadImage(PlayerPrefs.GetString(name, ""));
        inputField.onValueChanged.AddListener(TextChanged);
    }

    public void PickImage()
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            LoadImage(path);
        });

        Debug.Log("Permission result: " + permission);
    }

    void LoadImage(string path)
    {
        if (path == null || path == "") return;
        Debug.Log(name + path);

        // Create Texture from selected image
        Texture2D texture = NativeGallery.LoadImageAtPath(path);
        if (texture == null)
        {
            Debug.Log("Couldn't load texture from " + path);
            return;
        }

        dimensions = new Vector2(texture.width, texture.height);
        dimensions = dimensions.normalized;
        dimensions *= idealScale;

        image.texture = texture;
        material.mainTexture = texture;

        AppManager.instance.currentArObject = arObject;
        AppManager.instance.CreateNewARObjectInstance(AppManager.instance.currentArObject, transform);
        AppManager.instance.DestroyCurrentArObjInstance();

        PlayerPrefs.SetString(name, path);
    }

    public void NewArObjInstance()
    {
        Debug.Log("new");
        if (AppManager.instance.currentArObjectInstance.ArObj == arObject)
        {
            GameObject obj = AppManager.instance.currentArObjectInstance.instance;
            obj.transform.GetChild(0).localScale = new Vector3(dimensions.x, dimensions.y, 1);
        }
    }

    public void TextChanged(string value)
    {
        foreach (var str in arObject.NamesInLanguages)
        {
            str.name = inputField.text;
        }
    }
}
