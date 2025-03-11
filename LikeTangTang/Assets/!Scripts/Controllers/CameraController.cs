using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    GameObject go;
    Vector3 camPos;

    private void Start()
    {
        camPos = transform.position;
    }
    private void LateUpdate()
    {
        transform.position = new Vector3(go.transform.position.x, go.transform.position.y, camPos.z);
    }
}
