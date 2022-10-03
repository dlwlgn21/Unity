using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailMover : MonoBehaviour
{
    public int moveSpeed;

    void Update()
    {
        Debug.Assert(moveSpeed > 0);
        transform.Translate((Vector3.right * Time.deltaTime) * moveSpeed);
        Destroy(gameObject, 1);
    }
}
