using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    //includes homing logic so projectiles dont miss targets

    private Transform target;

    public float speed = 20f;
    public int damage = 10;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        if (target == null) //if the enemy dies before the projectile reaches the them, the projectile is destroyed
        {
            Destroy(gameObject);
            return;
        }
        
        //calculating the direction of the enemy
        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        
        
        if (direction.magnitude < distanceThisFrame) // check if projectile will hit enemy in this frame
        {
            HitTarget();
            return;
        }
        
        transform.Translate(direction.normalized * distanceThisFrame, Space.World); //move projectile toward enemy
        transform.LookAt(target); //keeps projectile facing enemy if we add a model for it
        
    }

    void HitTarget()
    {
        EnemyType1 enemy = target.GetComponent<EnemyType1>(); //get enemytype1 script to access health and takedamage method
        
        if (enemy != null)
        {
            enemy.TakeDamage(damage); //call takedamage method inside of enemytype1
        }
        Destroy(gameObject); //destroy projectile after it hits the enemy + deals damage
        
    }
   
}
