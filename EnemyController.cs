using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float movingSpeed = 3.0f;
    public bool vertical;
    public float changeTime = 3.0f;

    Rigidbody2D rb2D;
    float timer;
    int direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }

        Vector2 pos = rb2D.position;

        if (vertical)
        {
            pos.y = pos.y + (Time.deltaTime * movingSpeed * direction);
        }
        else
        {
            pos.x = pos.x + (Time.deltaTime * movingSpeed * direction);
        }
        rb2D.MovePosition(pos);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RubyController player = collision.gameObject.GetComponent<RubyController>();            // Reason Why collision.gameObject is Not Colider2D. 
        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }
}
