using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotator : MonoBehaviour
{
    void Update()
    {
        // Local->World Transfrom. Get Arm->mouse diff.
        // Camera.main.ScreenToWorldPoint(Input.mousePosition) -> Get MouseCursor World Position 
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();

        float rotZDegree = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotZDegree);
    }
}
