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

    void Start()
    {
        Debug.Assert(healthBarRect != null);
        Debug.Assert(healthText != null);
    }

    void Update()
    {
        
    }

    public void SetHealth(int current, int max)
    {
        
        float value = (float)current / max;

        healthBarRect.localScale = new Vector3(value, healthBarRect.localScale.y, healthBarRect.localScale.z);
        healthText.text = $"{current} / {max} HP";
    }
}
