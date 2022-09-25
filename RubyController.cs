using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public float movingSpeed = 3.0f;
    public int maxHealth = 5;
    public int health { get { return currentHealth; } }
    int currentHealth;

    Rigidbody2D rb2D;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        currentHealth = 1;
    }

    // Update is called once per frame
    void Update()
    {
        float xDir = Input.GetAxis("Horizontal");
        float yDir = Input.GetAxis("Vertical");
        Vector2 pos = rb2D.position;
        pos.x += (movingSpeed * xDir * Time.deltaTime);
        pos.y += (movingSpeed * yDir * Time.deltaTime);

        rb2D.MovePosition(pos);
    }


    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log($"{currentHealth} / {maxHealth}");
    }
}
