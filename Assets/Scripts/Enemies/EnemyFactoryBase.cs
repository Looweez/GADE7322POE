using UnityEngine;

public abstract class EnemyFactoryBase : MonoBehaviour
{
    public abstract EnemyBase CreateAnt(Vector3 pos);
    public abstract EnemyBase CreateFly(Vector3 pos);
    public abstract EnemyBase CreateRat(Vector3 pos);
}
