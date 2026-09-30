using UnityEngine;

public class EnemyRat : EnemyBase
{
    private void Awake()
    {
        // heavy rat enemy
        speed = 1.5f;
        EnemyMaxHealth = 150f;
        //damageToTower = 25;  
        attackDamage = 20;
        attackRange = 0.5f;
        attackInterval = 4f;

        Initialize();
    }
}
