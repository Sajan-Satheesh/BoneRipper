using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Random = UnityEngine.Random;

public class EnemyService : GenericSingleton<EnemyService>
{
    [field: SerializeField] public Transform enemyRoot { get; private set; }
    [SerializeField] int arrowShootableEnemies;
    [SerializeField] int defaultEnemyCount;
    [SerializeField] public float groundEnemyMinAttackRadius;
    private int enemyCount;
    [SerializeField] float groundEnemySpwanInterval;
    [SerializeField] GroundEnemyData groundEnemyConfig;
    [SerializeField] TowerEnemyData roofEnemyConfig;
    [field : SerializeField] public LayerMask sightHiddingLayer { get; private set; }
    public EventsEnemy events = new();


    private List<GroundEnemyController> groundEnemies { get; set; } = new List<GroundEnemyController>();
    private List<TowerEnemyController> roofEnemies { get; set; } = new List<TowerEnemyController>();
    private List<Transform> groundEnemySpawnTransforms { get; set; } = new List<Transform>();
    private List<Vector3> roofEnemySpawnPos { get; set; } = new List<Vector3>();
    Coroutine SpawnEnemies { get; set; }
    Coroutine randomShooting { get; set; }

    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        PlayerService.instance.events.Subscribe_OnReachingLand(reactOnPlayerInIsland_Player);
        PlayerService.instance.events.Subscribe_OnGameOver(reactOnGameOver);
        WorldService.instance.events.Subscribe_OnExitTrigger(reactOnPlayerExit);
        WorldService.instance.events.Subscribe_OnNewLevel(reactOnNewLevel);
        enemyCount = defaultEnemyCount + 5 * requestLevelCount();
    }

    private void reactOnNewLevel()
    {
        enemyCount = defaultEnemyCount + 5 * requestLevelCount();
    }

    int requestLevelCount()
    {
        return WorldService.instance.getLevelNum();
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
            enemy.setState(EnumGroundEnemyStates.RESTING);
        }
        foreach (TowerEnemyController enemy in roofEnemies)
        {
            enemy.setState(EnumTowerEnemyStates.RESTING);
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
        randomShooting = StartCoroutine(randomRoofEnemyAttack(arrowShootableEnemies));
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
                roofEnemies[index].setState(EnumTowerEnemyStates.ATTACKING);
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
        if (roofEnemySpawnPos == null) return;
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

    Transform randomSpawnPoint( List<Transform> spawnPoints)
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
            BoneServices.instance.spawnBones(enemy.getBodyParts());
            destroyGroundEnemy(enemy);
            groundEnemies.Remove(enemy);
        }
        events.InvokeOnEnemyDestroyed();
    }


    private void destroyGroundEnemy(GroundEnemyController enemy)
    {
        enemy.destructEnemy();
        enemy = null;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        PlayerService.instance.events.UnSubscribe_OnReachingLand(reactOnPlayerInIsland_Player);
        PlayerService.instance.events.UnSubscribe_OnGameOver(reactOnGameOver);
        WorldService.instance.events.UnSubscribe_OnExitTrigger(reactOnPlayerExit);
        WorldService.instance.events.UnSubscribe_OnNewLevel(reactOnNewLevel);
    }

    public Vector3 requestLandCenter()
    {
        return WorldService.instance.getLandPosition();
    }
}