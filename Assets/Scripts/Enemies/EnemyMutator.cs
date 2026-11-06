using UnityEngine;

public class EnemyMutator : MonoBehaviour
{
    public enum MutationType
    {
        Standard,
        Giant,
        Swarm,
        Bioluminescent,
        Enraged,
        Camouflage
    }

    [Header("Current Mutation")]
    public MutationType activeMutation = MutationType.Standard;

    [Header("Point Cost Modifier")]
    public int basePointCost = 10;

    private EnemyBase enemyBase;
    private Renderer enemyRenderer;
    private Light pointLight;

    private void Awake()
    {
        enemyBase = GetComponent<EnemyBase>();
        enemyRenderer = GetComponentInChildren<Renderer>();
        pointLight = GetComponentInChildren<Light>();

        // ensure a point light exists as a child or component
        if (pointLight == null)
        {
            GameObject lightObj = new GameObject("MutationLight");
            lightObj.transform.SetParent(transform);
            lightObj.transform.localPosition = Vector3.up * 0.5f;
            pointLight = lightObj.AddComponent<Light>();
            pointLight.type = LightType.Point;
            pointLight.range = 3f;
            pointLight.intensity = 2f;
            pointLight.enabled = false;
        }
    }

    public void ApplyRandomMutation()
    {
        // randomly pick  mutation type
        int roll = Random.Range(0, 6);
        activeMutation = (MutationType)roll;

        // stat and visual modifications based on  roll
        switch (activeMutation)
        {
            case MutationType.Giant:
                transform.localScale = Vector3.one * 1.5f;
                if (enemyBase != null)
                {
                    enemyBase.EnemyMaxHealth *= 1.8f;
                    enemyBase.EnemyCurrentHealth = enemyBase.EnemyMaxHealth;
                    enemyBase.speed *= 0.7f; //slower
                }
                break;

            case MutationType.Swarm:
                transform.localScale = Vector3.one * 0.65f;
                if (enemyBase != null)
                {
                    enemyBase.EnemyMaxHealth *= 0.5f;
                    enemyBase.EnemyCurrentHealth = enemyBase.EnemyMaxHealth;
                    enemyBase.speed *= 1.4f; //faster
                }
                break;

            case MutationType.Bioluminescent:
                if (enemyBase != null)
                {
                    enemyBase.speed *= 1.35f;
                }
                if (pointLight != null)
                {
                    pointLight.enabled = true;
                    pointLight.color = Color.green; //glow
                }
                break;

            case MutationType.Enraged:
                if (enemyRenderer != null)
                {
                    //tint  material  red
                    enemyRenderer.material.color = new Color(0.8f, 0.1f, 0.1f);
                }
                if (enemyBase != null)
                {
                    enemyBase.attackDamage *= 1.5f; 
                }
                break;

            case MutationType.Camouflage:
                if (enemyRenderer != null)
                {
                    //material semi-translucent
                    Color col = enemyRenderer.material.color;
                    col.a = 0.45f;
                    enemyRenderer.material.color = col;
                }
                break;

            case MutationType.Standard:
            default:
                //Normal stats no changes
                break;
        }

        Debug.Log($"spawned with mutation: {activeMutation}");
    }

    //Returns the adjusted point value for PointPool system
    public int GetModifiedPointCost()
    {
        switch (activeMutation)
        {
            case MutationType.Giant: return Mathf.RoundToInt(basePointCost * 1.8f);
            case MutationType.Swarm: return Mathf.RoundToInt(basePointCost * 0.7f);
            case MutationType.Bioluminescent: return Mathf.RoundToInt(basePointCost * 1.3f);
            case MutationType.Enraged: return Mathf.RoundToInt(basePointCost * 1.4f);
            case MutationType.Camouflage: return Mathf.RoundToInt(basePointCost * 1.2f);
            default: return basePointCost;
        }
    }
}
