using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
   
    public EnemySpawner enemySpawner;
  
    public float timeBetweenWaves = 5f; //time between waves
    private bool waveInProgress = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        //start wave
        StartCoroutine(StartNextWaveDelayed(3f));
    }

    IEnumerator StartNextWaveDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartNextWave();
    }

    public void StartNextWave()
    {
        if (waveInProgress) return;

        waveInProgress = true;
        
        //show wave number
        if (UIController.Instance != null)
        {
            UIController.Instance.UpdateWaveText(enemySpawner.currentWave);
        }

        //tell enemyspawner to start spawning with point pool
        enemySpawner.StartSpawningWave();
        
        StartCoroutine(MonitorWaveProgress());
    }

    IEnumerator MonitorWaveProgress()
    {
      
        yield return new WaitForSeconds(2f);
        
        while (true)
        {
            EnemyBase[] remainingEnemies = FindObjectsOfType<EnemyBase>();
            
            if (remainingEnemies.Length == 0)
            {
                break;
            }

            yield return new WaitForSeconds(1f);
        }

        //wave finsiehd
        enemySpawner.EvaluateWavePerformance();

        waveInProgress = false;
        Debug.Log("Wave completed");
        
        yield return new WaitForSeconds(timeBetweenWaves);
        StartNextWave();
    }
}