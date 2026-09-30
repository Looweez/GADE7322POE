using System;
using UnityEngine;

public class EnemyFly : EnemyBase
{
    public GameObject spitPrefab;
    public Transform spitSpawnPoint;

    private void Awake()
    {
        speed = 4.5f;
        EnemyMaxHealth = 25f;
        attackDamage = 8f;
        attackRange = 8f;
        attackInterval = 1.8f;
    }
    
    protected override void PerformAttack(TowerHealth tower)
    {
        ShootProjectile(tower.transform);
    }

    protected override void PerformAttack(DefenderBase defender)
    {
        ShootProjectile(defender.transform);
    }

    private void ShootProjectile(Transform target)
    {
        if (spitPrefab == null) return;

        // uses set spawn point, otherwise uses the fly's location
        Vector3 spawnPos = (spitSpawnPoint != null) ? spitSpawnPoint.position : transform.position;
        GameObject projObj = Instantiate(spitPrefab, spawnPos, Quaternion.identity);

        // aims at target
        Vector3 targetDirection = target.position - spawnPos;
        if (targetDirection != Vector3.zero)
        {
            projObj.transform.rotation = Quaternion.LookRotation(targetDirection);
        }

        // pasees the damage to the spit projectile script
        if (projObj.TryGetComponent<SpitProjectile>(out SpitProjectile proj))
        {
            proj.damage = attackDamage;
        }
    }
    
}