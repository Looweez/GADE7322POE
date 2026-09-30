using UnityEngine;

public abstract class ConcreteEnemyFactory : EnemyFactoryBase
{
    public Transform player; 

    [Header("Prefabs")]
    public GameObject antPrefab;
    public GameObject flyPrefab;
    public GameObject ratPrefab;

    public override EnemyBase CreateAnt(Vector3 pos)
    {
        GameObject obj = Instantiate(antPrefab, pos, Quaternion.identity);
        EnemyBase antBoi = obj.GetComponent<EnemyBase>();
        
        /*antBoi.speed = 3f;
        antBoi.EnemyMaxHealth = 100f;
        antBoi.EnemyCurrentHealth = 100f;*/
        
        antBoi.Initialize();
        return antBoi;
    }

    public override EnemyBase CreateFly(Vector3 pos)
    {
        GameObject obj = Instantiate(flyPrefab, pos, Quaternion.identity);
        EnemyBase flyBoi = obj.GetComponent<EnemyBase>();
        
        /*flyBoi.speed = 3f;
        flyBoi.EnemyMaxHealth = 100f;
        flyBoi.EnemyCurrentHealth = 100f;*/
        
        flyBoi.Initialize();
        return flyBoi;
    }
    
    public override EnemyBase CreateRat(Vector3 pos)
    {
        GameObject obj = Instantiate(ratPrefab, pos, Quaternion.identity);
        EnemyBase ratGurl = obj.GetComponent<EnemyBase>();
        
        /*ratGurl.speed = 3f;
        ratGurl.EnemyMaxHealth = 100f;
        ratGurl.EnemyCurrentHealth = 100f;*/
        
        ratGurl.Initialize();
        return ratGurl;
    }
    
}
