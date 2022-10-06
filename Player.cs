using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(UnityStandardAssets._2D.Platformer2DUserControl))]
public class Player : MonoBehaviour
{
    private float deathLimitHeight = -20;

    [SerializeField]
    private StatusIndicator mStatusIndicator;

    [SerializeField]
    private string[] playerGruntSounds;

    [SerializeField]
    private string playerFootStepSound;

    private PlayerStats mPlayerStasts;

    private void Start()
    {
        mPlayerStasts = PlayerStats.instance;
        Debug.Assert(mPlayerStasts != null);
        Debug.Assert(mStatusIndicator != null);
        mStatusIndicator.SetHealth(mPlayerStasts.CurrentHealth, mPlayerStasts.MaxHealth);
        GameMaster.gm.onToggleUpgrademenu += OnUpgradeMenuToggle;

        InvokeRepeating("regenHealth", mPlayerStasts.HelathRegenRate, mPlayerStasts.HelathRegenRate);
    }
    

    void Update()
    {
        if (transform.position.y <= deathLimitHeight)
        {
            DamagePlayer(1000);
        }
    }

    public void DamagePlayer(int damage)
    {
        mPlayerStasts.CurrentHealth -= damage;
        if (mPlayerStasts.CurrentHealth > 0)
        {
            AudioManager.instance.PlaySound(playerGruntSounds[Random.Range(0, 2)]);
        }
        else
        {
            GameMaster.gm.KillPlayer(this);
        }
        mStatusIndicator.SetHealth(mPlayerStasts.CurrentHealth, mPlayerStasts.MaxHealth);
    }

    void OnUpgradeMenuToggle(bool active)
    {
        GetComponent<UnityStandardAssets._2D.Platformer2DUserControl>().enabled = !active;
        Weapons weapon = GetComponentInChildren<Weapons>();
        if (weapon != null)
        {
            weapon.enabled = !active;
        }
    }

    private void regenHealth()
    {
        mPlayerStasts.CurrentHealth += 1;
        mStatusIndicator.SetHealth(mPlayerStasts.CurrentHealth, mPlayerStasts.MaxHealth);
    }

    void OnDestroy()
    {
        GameMaster.gm.onToggleUpgrademenu -= OnUpgradeMenuToggle;
    }

}
