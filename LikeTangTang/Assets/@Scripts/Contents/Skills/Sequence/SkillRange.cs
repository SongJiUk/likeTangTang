using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillRange : MonoBehaviour
{
    SpriteRenderer squre;
    ParticleSystem circle;
    ParticleSystem circle2;

    private void Awake()
    {
        squre = transform.GetChild(0).GetComponent<SpriteRenderer>();
        circle = Utils.FindChild<ParticleSystem>(gameObject, recursive: true);
        circle2 = circle.transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        squre.size = Vector2.zero;
    }

    public void SetInfo(Vector2 _dir, Vector2 _target, float _dist)
    {
        float distance = _dist;
        squre.size = new Vector2(1.3f, distance);

        Vector3 nomalDir = _dir.normalized;
        float angle = Mathf.Atan2(nomalDir.y, nomalDir.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public float SetCircle(float _startSize)
    {
        circle.Play();
        var main = circle.main;
        var main2 = circle2.main;
        main.startSize = _startSize;
        main2.startSize = _startSize;

        return circle.main.duration;
    }
}
