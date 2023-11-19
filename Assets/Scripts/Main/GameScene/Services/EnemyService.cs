using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyService : GenericSingleton<EnemyService>
{

    Coroutine SpawnEnemies;
    [SerializeField] int enemyCount;
    [SerializeField] float enemySpwanInterval;
    [SerializeField] SO_Enemy enemyConfig;
    [SerializeField] List<EnemyController> enemies;
    [field : SerializeField] public Transform enemyRoot { get; private set; }
    
    [SerializeField] private Transform[] allSpawnLocations;

    private void Start()
    {
        SpawnEnemies = StartCoroutine(createEnemies());
    }
    
    IEnumerator createEnemies()
    {
        for(int i=0; i < enemyCount; i++)
        {
            enemies.Add(new EnemyController(enemyConfig, randomSpawnPoint()));
            yield return new WaitForSeconds(enemySpwanInterval);
        }
    }

    Vector3 randomSpawnPoint()
    {
        int randomIndex= Random.Range(0, allSpawnLocations.Length);
        return allSpawnLocations[randomIndex].position;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}