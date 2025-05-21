using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Map : MonoBehaviour
{
    [SerializeField]
    public SpriteRenderer BackGround;

    public GridController Grid;
    [SerializeField]
    public GameObject MagneticField;


    public Vector2 MapSize
    {
        get { return BackGround.size; }
        set
        {
            BackGround.size = value;
        }

    }
    public void init()
    {
        Manager.GameM.CurrentMap = this;
        Grid.Init();
    }

    public void MagneticFieldReduction()
    {
        if (Manager.GameM.CurrentWaveIndex > 7) return;

        Vector3 baseSize = Vector3.one * 20f;

        float reductionRate = (10 - Manager.GameM.CurrentWaveIndex) * 0.1f;
        Vector3 targetScale = baseSize * reductionRate;
    }

    public void ChangeMapSize(float _targetRate, float _time = 120)
    {
        Vector3 currentSize = Vector3.one * 20f;
        if (Manager.GameM.CurrentWaveIndex > 7) return;

        MagneticField.transform.DOScale(currentSize * (10 - Manager.GameM.CurrentWaveIndex) * 0.1f, 3);
    }
}
