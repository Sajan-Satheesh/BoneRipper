using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class EnemyService : GenericSingleton<EnemyService>
{
    [field: SerializeField] public Transform enemyRoot { get; private set; }
    [SerializeField] int enemyCount;
    [SerializeField] float groundEnemySpwanInterval;
    [SerializeField] SO_GroundEnemy groundEnemyConfig;
    [SerializeField] SO_TowerEnemy roofEnemyConfig;
    private List<GroundEnemyController> groundEnemies { get; set; } = new List<GroundEnemyController>();
    private List<TowerEnemyController> roofEnemies { get; set; } = new List<TowerEnemyController>();
    private List<Transform> groundEnemySpawnTransforms { get; set; } = new List<Transform>();
    private List<Vector3> roofEnemySpawnPos { get; set; } = new List<Vector3>();
    Coroutine SpawnEnemies { get; set; }
    Coroutine randomShooting { get; set; }


    private void Start()
    {
        PlayerService.instance.onReachingLand += reactOnPlayerInIsland_Player;
        PlayerService.instance.onGameOver += reactOnGameOver;
        WorldService.instance.onExitTrigger += reactOnPlayerExit;
    }

    private void reactOnPlayerExit()
    {
        reactOnGameOver();
    }

    private void stopCoroutine(Coroutine coroutine)
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }
    private void stopCoroutines( Coroutine[] coroutines)
    {
        for (int i = 0; i < coroutines.Length; i++)
        {
            if (coroutines[i] != null)
            {
                StopCoroutine(coroutines[i]);
                coroutines[i] = null;
            }
        }
    }

    private void reactOnGameOver()
    {
        stopCoroutines(new Coroutine[]{ randomShooting, SpawnEnemies });
        foreach (GroundEnemyController enemy in groundEnemies)
        {
            enemy.setState(GroundEnemyState.resting);
        }
        foreach (TowerEnemyController enemy in roofEnemies)
        {
            enemy.setState(TowerEnemyState.resting);
        }
    }

    private void reactOnPlayerInIsland_Player()
    {
        generateEnemies();
    }
    private void generateEnemies()
    {
        requestGroundEnemySpawnTransform();
        requestRoofEnemySpawnPos();
        startSpawningGroundEnemies();
        spawnRoofEnemies();
    }

    void startRandomAttackOnPlayer()
    {
        if (randomShooting != null)
        {
            stopCoroutine(randomShooting);
        }
        randomShooting = StartCoroutine(randomRoofEnemyAttack(1));
    }

    IEnumerator randomRoofEnemyAttack(int attackingEnemyCount)
    {
        yield return new WaitForSeconds(5f);
        List<int> randomIndexes = new List<int>();
        int randomIndex = 0;
        while (roofEnemies.Count> 0)
        {
            if(attackingEnemyCount>roofEnemies.Count) { attackingEnemyCount = roofEnemies.Count; }
            while(randomIndexes.Count < attackingEnemyCount)
            {
                randomIndex = randomRoofEnemyIndex();
                if (!randomIndexes.Contains(randomIndex)) randomIndexes.Add(randomIndex);
            }
            foreach (int index in randomIndexes)
            {
                roofEnemies[index].prepareAttack();
            }
            yield return new WaitForSeconds(2f);
            randomIndexes.Clear();

        }
        yield return null;
        int randomRoofEnemyIndex()
        {
            if(roofEnemies.Count<=0) return -1;
            return Random.Range(0,roofEnemies.Count);
        }
    }
    private void startSpawningGroundEnemies()
    {
        if(SpawnEnemies != null) stopCoroutine(SpawnEnemies);
        SpawnEnemies = StartCoroutine(spawnGroundEnemies());
    }

    

    private void spawnRoofEnemies()
    {
        if (roofEnemies.Count > 0) return;
        for (int i = 0; i < roofEnemySpawnPos.Count; i++)
        {
            roofEnemies.Add(new TowerEnemyController(roofEnemyConfig, roofEnemySpawnPos[i]));
            roofEnemies[i].setEnemyRoot(groundEnemySpawnTransforms[i]);
        }
        startRandomAttackOnPlayer();
    }
    
    IEnumerator spawnGroundEnemies()
    {
        if (groundEnemies.Count > 0) StopCoroutine(spawnGroundEnemies());
        for (int i=0; i < enemyCount; i++)
        {
            groundEnemies.Add(new GroundEnemyController(groundEnemyConfig, randomSpawnPoint(groundEnemySpawnTransforms)));
            yield return new WaitForSeconds(groundEnemySpwanInterval);
        }
    }

    Transform randomSpawnPoint(List<Transform> spawnPoints)
    {
        int randomIndex= Random.Range(0, spawnPoints.Count);
        return spawnPoints[randomIndex];
    }

    private void requestGroundEnemySpawnTransform()
    {
        groundEnemySpawnTransforms = WorldService.instance.getAllHideOutTransform();
    }

    private void requestRoofEnemySpawnPos()
    {
        roofEnemySpawnPos = WorldService.instance.getAllRoofPos();
    }

    public Vector3 requestPlayerLocation()
    {
        return PlayerService.instance.getPlayerLocation() + Vector3.up * 0.5f;
    }

    public float requestPlayerSpeed()
    {
        return PlayerService.instance.getPlayerSpeed();
    }

    public Vector3 requestPlayerDirection()
    {
        return PlayerService.instance.getPlayerDirection();
    }

    public void destroyAllEnemies()
    {
        groundEnemies.ForEach(destroyGroundEnemy);

        roofEnemies.Clear();
        groundEnemies.Clear();
        groundEnemySpawnTransforms.Clear();
        roofEnemySpawnPos.Clear();
    }

    public void getHitGroundEnemy(GroundEnemyController enemy)
    {
        if (groundEnemies.Contains(enemy))
        {
            destroyGroundEnemy(enemy);
            groundEnemies.Remove(enemy);
        }
    }


    private void destroyGroundEnemy(GroundEnemyController enemy)
    {
        enemy.destructEnemy();
        enemy = null;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }


}