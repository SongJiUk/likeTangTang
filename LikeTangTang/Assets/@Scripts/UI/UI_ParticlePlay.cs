using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using DG.Tweening;

public class UI_ParticlePlay : MonoBehaviour
{
    [SerializeField]
    UIParticleSystem particle;

    private void OnEnable()
    {
        particle.DOPlay();
    }
}
