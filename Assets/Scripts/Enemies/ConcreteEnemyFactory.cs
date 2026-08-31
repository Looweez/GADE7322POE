using UnityEngine;

public class ConcreteEnemyFactory : EnemyFactoryBase
{
    public Transform player; 

    [Header("Prefabs")]
    public GameObject antPrefab;

    public override EnemyBase CreateAnt(Vector3 pos)
    {
        GameObject obj = Instantiate(antPrefab, pos, Quaternion.identity);
        EnemyBase antBoi = obj.GetComponent<EnemyBase>();
        
        antBoi.speed = 3f;
        antBoi.EnemyMaxHealth = 100f;
        antBoi.EnemyCurrentHealth = 100f;
        
        antBoi.Initialize();
        return antBoi;
    }
    
}
