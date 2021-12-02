using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public class MultiUiInfo : MonoBehaviour
{
    public TMPro.TextMeshProUGUI playerChipsTxt, playerBetAmount;
    public Image playerPickImage;

    private void Awake()
    {
        //spwan the prefab to HUD gameobject.
        transform.SetParent(GameObject.FindGameObjectWithTag("PlayerHUD").GetComponent<Transform>(), false);
    }
}
