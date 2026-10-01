using UnityEngine;

public class ConcreteDefenderFactory : DefenderFactoryBase
{
    [Header("Prefabs")]
    public GameObject cupcakePrefab;
    public GameObject jellyTotPrefab;
    public GameObject snowballPrefab;
    
    public override DefenderBase CreateCupcake(Vector3 pos)
    {
        GameObject obj = Instantiate(cupcakePrefab, pos, Quaternion.identity);
        DefenderBase cupcake = obj.GetComponent<DefenderBase>();
        
        cupcake.speed = 0f;
        cupcake.defenderCurrentHealth = 100f;
        cupcake.defenderMaxHealth = 100f;
        
        cupcake.Initialize();
        return cupcake;
    }

    public override DefenderBase CreateJellyTot(Vector3 pos)
    {
        GameObject obj = Instantiate(jellyTotPrefab, pos, Quaternion.identity);
        DefenderBase jellyTot = obj.GetComponent<DefenderBase>();
        
        jellyTot.speed = 0f;
        jellyTot.defenderCurrentHealth = 60f; // Lower health, fast shooter
        jellyTot.defenderMaxHealth = 60f;
        
        jellyTot.Initialize();
        return jellyTot;
    }

    public override DefenderBase CreateSnowball(Vector3 pos)
    {
        GameObject obj = Instantiate(snowballPrefab, pos, Quaternion.identity);
        DefenderBase snowball = obj.GetComponent<DefenderBase>();
        
        snowball.speed = 0f;
        snowball.defenderCurrentHealth = 150f; // Sturdier tank defender
        snowball.defenderMaxHealth = 150f;
        
        snowball.Initialize();
        return snowball;
    }
}