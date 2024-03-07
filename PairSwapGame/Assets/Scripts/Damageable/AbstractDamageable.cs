using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.U2D;

public abstract class AbstractDamageable : MonoBehaviour
{
    public static System.Random rand = new System.Random();
    [SerializeField] private SpriteShapeRenderer spriteRenderer;
    #region Health Stuff
    [SerializeField] protected int Health = 50;
    public EEnemyType enemyType;
    public abstract void TakeDamage(int dmg, Vector2 impactDirection);
    protected virtual void Died()
    {
        ScoreManager.Instance.IncreaseScore(enemyType);
        ObjectPoolManager.ReturnObjectToPool(gameObject, (int)EPoolableObjectType.Enemy, (int)enemyType);
    }
    #endregion


    #region Jitter Animation
    private Vector2 originalPosition;
    private const float jitterDistance =  0.1f; // The distance to jitter
    private const float jitterDuration =  0.1f; // The duration of the jitter animation
    private const float returnDuration =  0.1f; // The duration to return to the original position
    
    public void Jitter(Vector2 direction)
    {
        if(Health <= 0) return;
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

    #region Color
    private static readonly Color[] healthColors = new Color[] { Color.black, Color.red, new Color(255, 180, 0), Color.green, Color.cyan };
    private int currentColorIndex = 0;
    
    protected void SwitchColorIndex(int health)
    {
        int newIndex = 0;
        switch (health)
        {
            case int n when n >= 0 && n <= 5:
                newIndex = 4;
                break;
            case int n when n > 5 && n <= 15:
                newIndex = 3;
                break;
            case int n when n > 15 && n <= 25:
                newIndex = 2;
                break;
            case int n when n > 25 && n <= 35:
                newIndex = 1;
                break;
            case int n when n > 35:
                newIndex = 0;
                break;
            default:
                newIndex = currentColorIndex;
                break;
        }
        if(newIndex != currentColorIndex)
        {
            currentColorIndex = newIndex;
            spriteRenderer.color = healthColors[currentColorIndex];
        }
    }
    #endregion

    #region  Setup
    public AnimationCurve moveCurve; // Define the animation curve in the Unity Editor
    public void SetUp(int health, Vector2 targetPos)
    {
        Health = health;
        SwitchColorIndex(health);
        StartCoroutine(MoveToPosition(targetPos, 1f));
    }
    #endregion


    IEnumerator MoveToPosition(Vector3 target, float duration)
    {
        Vector3 start = transform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration; // Normalized time
            float curveValue = moveCurve.Evaluate(t); // Apply the animation curve
            transform.position = Vector3.Lerp(start, target, curveValue);
            yield return null; // Wait for the next frame
        }

        transform.position = target; // Ensure the final position is exactly the target
    }

    protected void DropPowerup()
    {
        PowerUpMaster.Instance.PickRandomPowerUp(transform.position, Quaternion.identity);
    }
}
