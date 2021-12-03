using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem ps;
    public void PlayEffect()
    {
        if (ps != null) ps.Play();
    }
}
