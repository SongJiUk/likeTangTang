using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Test : MonoBehaviour
{
    public float moveSpeed = 10f; // 이동 속도
    public float rotateSpeed = 360f; // 초당 몇도 회전할지

    private Vector3 moveDir;

    void Start()
    {
        //SetDirection(Vector3.right);
    }
    public void SetDirection(Vector3 direction)
    {
        moveDir = direction.normalized;
    }

    void Update()
    {
    }
}
