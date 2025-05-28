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
    public GameObject MagneticField; //TODO : 원래 5, 5 였음.


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

        Vector3 baseSize = Vector3.one * 30f;

        float reductionRate = (9 - Manager.GameM.CurrentWaveIndex) * 0.1f;
        Vector3 targetScale = baseSize * reductionRate;
        MagneticField.transform.DOScale(targetScale, 3f);

        //if (Manager.GameM.CurrentWaveIndex > 7) return;

        //// 카메라에서 보이는 월드 영역 구하기
        //Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 10f));
        //Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 10f));
        //Vector3 viewSize = topRight - bottomLeft;

        //// 기준 사이즈를 화면 기준으로 동적 계산
        //Vector3 baseSize = new Vector3(viewSize.x, viewSize.y, 1f) * 1.2f; // 1.2f는 여유

        //float reductionRate = (9 - Manager.GameM.CurrentWaveIndex) * 0.1f;
        //Vector3 targetScale = baseSize * reductionRate;

        //MagneticField.transform.position = Camera.main.transform.position; // 중앙 정렬
        //MagneticField.transform.DOScale(targetScale, 3f);
    }
}