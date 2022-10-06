using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class CameraShaker : MonoBehaviour
{

    private Camera mainCam;
    private float shakeAmount = 0.0f;

    private void Awake()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
        }
    }

    public void Shake(float amount, float length)
    {
        shakeAmount = amount;
        InvokeRepeating("doShake", 0, 0.01f);
        Invoke("stopShake", length);

    }

    private void doShake()
    {
        if (shakeAmount > 0)
        {
            Vector3 camPosition = mainCam.transform.position;
            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;
            camPosition.x += offsetX;
            camPosition.y += offsetY;

            mainCam.transform.position = camPosition;
        }
    }

    private void stopShake()
    {
        CancelInvoke("doShake");
        mainCam.transform.localPosition = Vector3.zero;
    }
}
