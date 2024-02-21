using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    [SerializeField] private Vector2 vectorForce;
    [SerializeField] private float force;
    private readonly static Vector2 zeroVector = new Vector2(0, 0);

    public void PewPew(Vector2 direction)
    {
        rb.velocity = zeroVector;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
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
