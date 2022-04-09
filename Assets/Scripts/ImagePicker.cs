using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagePicker : MonoBehaviour
{
    public ArObject arObject;
    public Material material;
    public RawImage image;
    public float idealScale;
    Vector2 dimensions = new Vector2();

    private void Start()
    {
        AppManager.instance.OnNewArObjInstance.AddListener(NewArObjInstance);

        LoadImage(PlayerPrefs.GetString("imagePath", ""));
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
        Debug.Log("Image path: " + path);

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

        PlayerPrefs.SetString("imagePath", path);
    }

    public void SpawnImage()
    {
        AppManager.instance.currentArObject = arObject;
        AppManager.instance.CreateNewARObjectInstance(AppManager.instance.currentArObject, transform);
        AppManager.instance.DestroyCurrentArObjInstance();

        AppManager.instance.CurrenState = AppState.PlaceState;
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
}
