using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public GameEnding gameEnding;

    public Transform player;
    bool mIsPlayerInRange;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            mIsPlayerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            mIsPlayerInRange = false;
        }
    }

    void Update()
    {
        if (mIsPlayerInRange)
        {
            Vector3 direction = player.position - transform.position + Vector3.up;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.transform == player)
                {
                    Debug.Log($"Observer hit : {hit.collider.gameObject}");
                    gameEnding.CaughtPlayer();
                }
            }
        }
            
    }
}
