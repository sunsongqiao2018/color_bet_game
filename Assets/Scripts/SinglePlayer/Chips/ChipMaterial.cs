using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipMaterial : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        Material mat = gameObject.GetComponentInChildren<MeshRenderer>().material;
        mat.SetColor("_Color", Random.ColorHSV(.5f, 1, .5f, 1, .5f, 1));
    }
}
