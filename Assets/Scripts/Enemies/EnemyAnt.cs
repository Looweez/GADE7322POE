using UnityEngine;

public class EnemyAnt : EnemyBase
{
    private void Start()
    {
        // ant general stat ememy
        speed = 3f;
        EnemyMaxHealth = 50f;
        damageToTower = 10;
        attackDamage = 10;

        Initialize();
    }
}