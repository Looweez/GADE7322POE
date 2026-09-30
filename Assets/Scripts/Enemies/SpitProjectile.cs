using UnityEngine;

public class SpitProjectile : MonoBehaviour
{
    [Header("Projectile Movement")]
    public float speed = 12f;
    public float lifeSpan = 5f;

    [HideInInspector] 
    public float damage = 8f; 

    private void Start()
    {
        // in case it shoots nothing fam
        Destroy(gameObject, lifeSpan);
    }

    private void Update()
    {
        // moves in the direction its facing
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // did it hit the tower?
        if (other.CompareTag("Tower"))
        {
            if (other.TryGetComponent<TowerHealth>(out TowerHealth tower))
            {
                tower.TakeDamage(damage);
            }
            Destroy(gameObject); // destroys after hitting tower
        }
        // did it hit a defender?
        else if (other.CompareTag("Defender"))
        {
            if (other.TryGetComponent<DefenderBase>(out DefenderBase defender))
            {
                defender.TakeDamage(damage);
            }
            Destroy(gameObject); // same ting but for defender
        }
    }
}
