using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyService : MonoBehaviour
{
    [SerializeField] int enemyCount = 10;
    Coroutine SpawnEnemies;
    [SerializeField] Mock_Enemy enemy;
    [SerializeField] float enemySpwanInterval;
    public static EnemyService instance;
    [SerializeField] GameObject Locations;
    [SerializeField] private Transform[] allSpawnLocations;

    private void Start()
    {
        allSpawnLocations = Locations.GetComponentsInChildren<Transform>();
        SpawnEnemies = StartCoroutine(createEnemies());
    }
    
    IEnumerator createEnemies()
    {
        for(int i=0; i < enemyCount; i++)
        {
            Mock_Enemy enemyInstance = Instantiate(enemy, randomSpawnPoint(), Quaternion.identity);
            enemyInstance.player = PlayerService.instance.Player;
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
