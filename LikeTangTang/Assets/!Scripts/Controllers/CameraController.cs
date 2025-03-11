using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    GameObject target;


    private void LateUpdate()
    {
        if (target != null)
            transform.position = target.transform.position;
    }
}
