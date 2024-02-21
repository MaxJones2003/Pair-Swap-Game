using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformHandler : MonoBehaviour
{
    public Camera cam;
    [SerializeField] private float yPosition = -8.5f;
    [SerializeField] private Vector2 xRange = new Vector2(-8.5f, 8.5f);

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 camPos = cam.ScreenToWorldPoint(mousePos);

        transform.position = ClampHeight(camPos);
        Debug.Log(rb.velocity);
    }


    Vector2 ClampHeight(Vector2 value)
    {
        value.y = yPosition;
        value.x = Mathf.Clamp(value.x, xRange.x, xRange.y);

        return value;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Projectile proj))
        {
            Debug.Log(proj.rb.velocity.sqrMagnitude);
        }
    }
}
