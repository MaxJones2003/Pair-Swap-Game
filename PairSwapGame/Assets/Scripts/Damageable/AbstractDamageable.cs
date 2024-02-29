using System.Collections;
using UnityEngine;

public abstract class AbstractDamageable : MonoBehaviour
{
    #region Health Stuff
    [SerializeField] protected int Health = 50;
    public abstract void TakeDamage(int dmg, Vector2 impactDirection);
    public abstract void Died();
    #endregion


    #region Jitter Animation
    private Vector2 originalPosition;
    private const float jitterDistance =  0.1f; // The distance to jitter
    private const float jitterDuration =  0.1f; // The duration of the jitter animation
    private const float returnDuration =  0.1f; // The duration to return to the original position
    
    public void Jitter(Vector2 direction)
    {
        originalPosition = transform.position;
        StartCoroutine(JitterAnimationCoroutine(direction));
    }

    private IEnumerator JitterAnimationCoroutine(Vector2 direction)
    {
        Vector2 targetPosition = originalPosition + direction.normalized * jitterDistance;
        float elapsedTime =  0f;

        // Move to the target position
        while (elapsedTime < jitterDuration)
        {
            transform.position = Vector2.Lerp(originalPosition, targetPosition, elapsedTime / jitterDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Return to the original position
        elapsedTime =  0f;
        while (elapsedTime < returnDuration)
        {
            transform.position = Vector2.Lerp(targetPosition, originalPosition, elapsedTime / returnDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    #endregion

    #region  Setup
    public void SetUp(int health)
    {
        Health = health;
    }
    #endregion
}
