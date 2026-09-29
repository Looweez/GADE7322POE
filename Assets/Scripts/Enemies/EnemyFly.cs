using UnityEngine;

public class EnemyFly : EnemyBase
{
    private void Start()
    {
        // light fly enemy
        speed = 5.5f;
        EnemyMaxHealth = 20f;
        damageToTower = 5;   
        attackDamage = 5;     

        Initialize();
    }
}