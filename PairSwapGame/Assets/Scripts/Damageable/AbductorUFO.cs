using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbductorUFO : AbstractDamageable
{
    private const int projectilesToSpawn = 4;
    [SerializeField] private TractorBeam tractorBeam;

    public void SpawnEnemyProjectiles()
    {
        float angleStep = 360f / projectilesToSpawn;
        for (int i = 0; i < projectilesToSpawn; i++)
        {
            // Calculate the angle for this projectile
            float angle = (i * angleStep + 45f) * Mathf.Deg2Rad;

            // Calculate the position using sine and cosine
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);

            // Convert to world position and instantiate the projectile
            Vector2 spawnPosition = new Vector3(x, y, 0) + transform.position;
            Projectile enemyProj =ObjectPoolManager.SpawnObject(ProjectileMaster.Instance.EnemyProjectile, spawnPosition, Quaternion.identity, (int)EPoolableObjectType.Projectile, (int)EProjectileType.Enemy).GetComponent<Projectile>();
            enemyProj.Fire((spawnPosition - (Vector2)transform.position).normalized);
        }
    }



    public override void TakeDamage(int dmg, Vector2 impactDirection)
    {
        Debug.Log("Adductor Take Damage: " + dmg);
        Health -= dmg;
        WaveManager.Instance.TotalEnemyHealth -= dmg;
        Jitter(impactDirection);
        if(Health <= 0) Died();
        else SwitchColorIndex(Health);
    }

    public void Die() => Died();

    protected override void Died()
    {
        if(tractorBeam.currentRigidbody != null)
        {
            Projectile proj = tractorBeam.currentRigidbody.GetComponent<Projectile>();
            if(tractorBeam.currentRigidbody.velocity.sqrMagnitude > 2)
            {
                proj.Fire(tractorBeam.currentRigidbody.velocity.normalized);
            }
            else
            {
                proj.Fire(Projectile.upVector);
            }
            tractorBeam.currentRigidbody = null;
            tractorBeam.StopAllCoroutines();
            tractorBeam.Abduct = null;
        }
        base.Died();
    }
}
