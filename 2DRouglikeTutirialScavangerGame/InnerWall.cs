using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerWall : MonoBehaviour
{
    public Sprite damagedSprite;
    public int hp = 4;

    private SpriteRenderer spriteRenderer;

    // AudioSection
    public AudioClip AttackedSound1;
    public AudioClip AttackedSound2;


    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void DamageWall(int loss)
    {
        spriteRenderer.sprite = damagedSprite;
        hp -= loss;
        SoundManager.instance.PlayRandomSfx(AttackedSound1, AttackedSound2);
        if (hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }

}
