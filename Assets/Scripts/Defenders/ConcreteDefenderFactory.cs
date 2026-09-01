using UnityEngine;

public class ConcreteDefenderFactory : DefenderFactoryBase
{
    [Header("Prefabs")]
    public GameObject cupcakePrefab;
    
    public override DefenderBase CreateCupcake(Vector3 pos)
    {
        GameObject obj = Instantiate(cupcakePrefab, pos, Quaternion.identity);
        DefenderBase cupcake = obj.GetComponent<DefenderBase>();
        
        cupcake.speed = 3f;
        cupcake.defenderCurrentHealth = 100f;
        cupcake.defenderMaxHealth = 100f;
        
        cupcake.Initialize();
        return cupcake;
    }
}
