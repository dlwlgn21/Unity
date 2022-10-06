using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UpgradeMunuManager : MonoBehaviour
{
    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Text speedText;

    [SerializeField]
    private float healthMultiplier;

    [SerializeField]
    private float movementSpeedMultiplier;

    [SerializeField]
    private int upgradeCost;

    [SerializeField]
    private string upgradeSuccessSound;

    [SerializeField]
    private string noMoneySound;


    private PlayerStats mPlayerStats;

    private void OnEnable()
    {
        mPlayerStats = PlayerStats.instance;
        Debug.Assert(mPlayerStats != null);
        updateValues();
    }
    void updateValues()
    {
        healthText.text = $"Health : {mPlayerStats.MaxHealth}";
        speedText.text = $"Speed : {mPlayerStats.MovementSpeed}";
    }

    public void UpgradeHealth()
    {
        if (GameMaster.Money >= upgradeCost)
        {
            Debug.Assert(mPlayerStats != null);
            mPlayerStats.MaxHealth = (int)(mPlayerStats.MaxHealth * healthMultiplier);

            GameMaster.Money -= upgradeCost;
            updateValues();
            Debug.Assert(upgradeSuccessSound != null);
            AudioManager.instance.PlaySound(upgradeSuccessSound);
            return;
        }
        Debug.Assert(noMoneySound != null);
        AudioManager.instance.PlaySound(noMoneySound);
    }

    public void UpgradeSpeed()
    {
        if (GameMaster.Money >= upgradeCost)
        {
            Debug.Assert(mPlayerStats != null);
            mPlayerStats.MovementSpeed = (int)(mPlayerStats.MovementSpeed * movementSpeedMultiplier);
        
            GameMaster.Money -= upgradeCost;
            updateValues();
            Debug.Assert(upgradeSuccessSound != null);
            AudioManager.instance.PlaySound(upgradeSuccessSound);
            return;
        }
        Debug.Assert(noMoneySound != null);
        AudioManager.instance.PlaySound(noMoneySound);

    }

}
