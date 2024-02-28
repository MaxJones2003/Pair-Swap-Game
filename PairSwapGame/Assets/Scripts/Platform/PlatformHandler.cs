using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlatformHandler : MonoBehaviour
{
    public Camera cam;
    [SerializeField] private float yPosition = -8.5f;
    [SerializeField] private Vector2 xRange = new Vector2(-8.5f, 8.5f);
    [SerializeField] private float speed = 1;
    private Vector2 lastPosition;
    private Vector2 velocity;
    [Range(0, 1)]
    public float biasFactor; // This factor can be adjusted to favor one vector over the other

    private static readonly Vector2 vectorZero = new Vector2(0, 0);

    private Vector2 preLerpVector;
    private const float lerpDuration = 0.05f;
    private float timeElapsed;
    void Update()
    {
        // Get the mouse position in world coordinates
        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = ClampHeight(mousePosition);
        // Calculate the velocity based on the change in position
        // The preLerpVector is almost always zero, so we only set velocity to it if it isn't zero
        // If the velocity is zero, we lerp the velocity to zero because the platform probably hasn't moved for a bit
        preLerpVector = (mousePosition - lastPosition) / Time.deltaTime;
        if (preLerpVector == vectorZero)
        {
            velocity = Vector2.Lerp(velocity, vectorZero, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
        }
        else
        {
            timeElapsed = 0;
            velocity = preLerpVector;
        }
        
        // Move the box to the mouse position
        transform.position = mousePosition;

        // Update the last position to the current mouse position
        lastPosition = mousePosition;
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
            if(velocity == Vector2.zero) return;
            velocity.x = Mathf.Clamp(velocity.x, -5, 5);
            proj.AddVelocity(velocity);            
        }
    }
}
