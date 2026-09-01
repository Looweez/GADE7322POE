using UnityEngine;

public abstract class DefenderFactoryBase : MonoBehaviour
{
    public abstract DefenderBase CreateCupcake (Vector3 pos);
    
    //public abstract DefenderBase CreateSnowball (Vector3 pos);
    
    //public abstract DefenderBase CreateJellytot (Vector3 pos);
}
