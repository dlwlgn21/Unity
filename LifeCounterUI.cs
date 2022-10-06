using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LifeCounterUI : MonoBehaviour
{
    private Text mLifeText;
    void Awake()
    {
        mLifeText = GetComponent<Text>();
    }


    void Update()
    {

        mLifeText.text = $"Lives: {GameMaster.RemainingLife}";
    }
}
