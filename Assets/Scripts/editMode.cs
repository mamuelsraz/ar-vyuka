using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Windows.Forms;

public class editMode : MonoBehaviour
{
    public TextMeshProUGUI statusText;
    public Image image;

    public void Choose()
    {
        Debug.Log(Clipboard.GetText());
    }

    public void Render()
    {

    }
}
