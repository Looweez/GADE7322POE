using UnityEngine;

public class EnemyAnt : EnemyBase
{
    private void Awake()
    {
        // ant general stat ememy
        speed = 3f;
        EnemyMaxHealth = 50f;
        //damageToTower = 10;
        attackDamage = 10f;
        attackRange = 1.2f;
        attackInterval = 1.2f;

        Initialize();
    }
}