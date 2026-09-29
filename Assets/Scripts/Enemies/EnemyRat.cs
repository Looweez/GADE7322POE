using UnityEngine;

public class EnemyRat : EnemyBase
{
    private void Start()
    {
        // heavy rat enemy
        speed = 1.5f;
        EnemyMaxHealth = 150f;
        damageToTower = 25;  
        attackDamage = 20;

        Initialize();
    }
}
