using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public class MultiUiInfo : MonoBehaviour
{
    public Text playerChipsTxt;
    public Image playerPickImage;
    private void Awake()
    {
        transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").GetComponent<Transform>(), false);
    }
}
