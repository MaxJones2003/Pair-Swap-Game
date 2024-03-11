using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropperUFO : AbstractDamageable
{
    [SerializeField, Range(0, 100)] private int spawnChance = 20;
    private const int dropSpeed = 2; // Adjust this value to control the speed of the drop




    public override void TakeDamage(int dmg, Vector2 impactDirection)
    {
        Health -= dmg;
        WaveManager.Instance.TotalEnemyHealth -= dmg;

        if(rand.Next(0, 100) < spawnChance)
            StartCoroutine(DropPowerUp());
        
        if(Health <= 0) Died();
        else SwitchColorIndex(Health);

        Jitter(impactDirection);
    }


    private IEnumerator DropPowerUp()
    {
        float dropHeight = UnityEngine.Random.Range(1.5f, 4f);

        Transform powerUp = PowerUpMaster.Instance.PickRandomPowerUp(transform.position, Quaternion.identity).transform;
        CircleCollider2D powerUpCollider = powerUp.GetComponent<CircleCollider2D>();
        powerUpCollider.enabled = false;
        // Calculate the target position
        Vector3 targetPosition = powerUp.position + new Vector3(0, -dropHeight, 0);

        // Calculate the duration of the drop based on the desired speed
        float duration = dropHeight / dropSpeed;

        // Start the drop
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            powerUp.position = Vector3.Lerp(powerUp.position, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the power-up ends up exactly at the target position
        powerUp.position = targetPosition;
        powerUpCollider.enabled = true;
    }
}
