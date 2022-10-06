using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    [SerializeField]
    private int initialMaxHealth;

    [SerializeField]
    private float initialSpeed;

    [SerializeField]
    private float maxSpeed;

    public int MaxHealth { get; set; }
    
    private int mCurrentHealth;
    public float HelathRegenRate { get; private set; }
    private float mMovementSpeed;
    public float MovementSpeed 
    {
        get { return mMovementSpeed; }
        set 
        { 
            if (value <= maxSpeed)
            {
                mMovementSpeed = value;
            }
        } 
    }
    public int CurrentHealth
    {
        get { return mCurrentHealth; }
        set { mCurrentHealth = Mathf.Clamp(value, 0, MaxHealth); }
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        MaxHealth = initialMaxHealth;
        CurrentHealth = MaxHealth;
        HelathRegenRate = 5.0f;
        MovementSpeed = initialSpeed;
    }

}
