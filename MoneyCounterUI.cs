using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class MoneyCounterUI : MonoBehaviour
{
    private Text mMoneyText;
    void Awake()
    {
        mMoneyText = GetComponent<Text>();
    }

    void Update()
    {
        mMoneyText.text = $"Money: {GameMaster.Money}";
    }
}
