using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageFont : MonoBehaviour
{
    TextMeshPro damageText;

    public void SetInfo(Vector2 _pos, float _damage, float _heal, Transform _parent, bool _isCritical = false)
    {
        damageText = GetComponent<TextMeshPro>();
        transform.position = _pos;

        if (_heal > 0)
        {
            damageText.text = $"{Mathf.RoundToInt(_heal)}";
            damageText.color = Utils.HexToColor("4EEE6F");
        }
        else if (_isCritical)
        {
            damageText.text = $"{Mathf.RoundToInt(_damage)}";
            damageText.color = Utils.HexToColor("EFAD00");
        }
        else
        {
            damageText.text = $"{Mathf.RoundToInt(_damage)}";
            damageText.color = Color.white;
        }

        damageText.alpha = 1;
        if (_parent != null)
           GetComponent<MeshRenderer>().sortingOrder = 300;

        DoAnim();
    }

    void DoAnim()
    {
        var tr = transform;
        var text = tr.GetComponent<TMP_Text>();
        Sequence sq = DOTween.Sequence();
        transform.localScale = Vector3.zero;

        sq.Append(tr.DOScale(1.3f, 0.3f).SetEase(Ease.InOutBounce))
            .Join(tr.DOMove(tr.position + Vector3.up, 0.3f).SetEase(Ease.Linear))
            .Append(tr.DOScale(1.0f, 0.3f).SetEase(Ease.InOutBounce))
            .Join(text.DOFade(0, 0.3f).SetEase(Ease.InQuint))
            .OnComplete(() =>
            {
                Manager.ResourceM.Destory(gameObject);
            });
    }
}
