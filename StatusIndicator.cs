using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StatusIndicator : MonoBehaviour
{

    [SerializeField]
    private RectTransform healthBarRect;

    [SerializeField]
    private Text healthText;


    private Vector3 mHealthCachingVector;
    void Start()
    {
        Debug.Assert(healthBarRect != null);
        Debug.Assert(healthText != null);
        mHealthCachingVector = new Vector3();
    }

    void Update()
    {
        
    }

    public void SetHealth(int current, int max)
    {
        
        float value = (float)current / max;
        mHealthCachingVector.Set(value, healthBarRect.localScale.y, healthBarRect.localScale.z);
        healthBarRect.localScale = mHealthCachingVector;
        healthText.text = $"{current} / {max} HP";
    }
}
