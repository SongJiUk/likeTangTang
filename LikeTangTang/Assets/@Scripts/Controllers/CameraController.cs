using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{


    public GameObject Target;
    public float Height { get; set; } = 0;
    public float Width { get; set; } = 0;

    float tickvalue = 5f;
    float adjust = 0.5f;
    bool isShake = false;
    Vector3 camPos;

    private void Start()
    {
        SetCameraSize();
    }
    private void LateUpdate()
    {
        if (Target != null && Manager.GameM.CurrentMap != null && !isShake)
            LimitCameraArea();
    }

    void SetCameraSize()
    {
        Camera.main.orthographicSize = 18f;
        Height = Camera.main.orthographicSize;
        Width = Height * Screen.width / Screen.height;
    }

    void LimitCameraArea()
    {
        transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, -10f);
        float limitX = Manager.GameM.CurrentMap.MapSize.x * 0.5f - Width;
        float clampX = Mathf.Clamp(transform.position.x, -limitX, limitX);

        float limitY = Manager.GameM.CurrentMap.MapSize.y * 0.5f - Height;
        float clampY = Mathf.Clamp(transform.position.y, -limitY, limitY);

        transform.position = new Vector3(clampX, clampY, -10f);
    }


    public void Shake()
    {
        if (!isShake)
            StartCoroutine(CoShake(0.25f));
    }

    IEnumerator CoShake(float _duration)
    {
        float halfDuration = _duration / 2;
        float elapsed = 0f;
        float tick = Random.Range(-10f, 10f);

        isShake = true;
        while(elapsed < _duration)
        {
            if (Manager.UiM.GetPopupCount() > 0) break;

            elapsed += Time.deltaTime / halfDuration;

            tick += Time.deltaTime * tickvalue;
            transform.position += new Vector3(
                Mathf.PerlinNoise(tick, 0) - 0.5f,
                Mathf.PerlinNoise(0, tick) - 0.5f,
                0f) * adjust * Mathf.PingPong(elapsed, halfDuration);

            yield return null;
        }

        isShake = false;
    }
}
