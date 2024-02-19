using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Vector2 vectorForce;
    [SerializeField] private float force;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.AddForce(transform.up * force, ForceMode2D.Impulse);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(rb.velocity.sqrMagnitude > 1000)
                rb.velocity = Vector2.zero;
            rb.AddForce(rb.velocity * force, ForceMode2D.Impulse);
        }
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            rb.velocity *= -1;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        float x = Random.Range(0f, 1f);
        float y = Random.Range(0f, 1f);
        float z = Random.Range(0f, 1f);
        Color c = new Color(x, y, z);

        spriteRenderer.color = c;
    }
}
