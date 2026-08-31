using UnityEngine;

public abstract class EnemyFactoryBase : MonoBehaviour
{
    public abstract EnemyBase CreateAnt(Vector3 pos);
}
