using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Target;
    Vector3 camPos;

    private void Start()
    {
        camPos = transform.position;
    }
    private void LateUpdate()
    {
        if(Target != null)
            transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, camPos.z);
    }
}
