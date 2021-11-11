using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class CardMaterial : MonoBehaviour
{
    //[SerializeField] Material redCardMat, greenCardMat;
    [SerializeField] Texture redTex, greenTex;
    Material _mat;
    private void Start()
    {
        _mat = gameObject.GetComponent<MeshRenderer>().material;
        StateMachine.Instance.BroadcastResult += SetCardMaterial;
    }
    public void SetCardMaterial(object sender, BoolEventArgs e)
    {
        bool result = e.value;
        _mat.SetTexture("_MainTex", result ? greenTex : redTex);
    }
}
