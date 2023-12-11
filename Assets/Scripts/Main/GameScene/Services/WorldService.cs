using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldService : GenericSingleton<WorldService>
{
    private LandGenerator landGenerator = new LandGenerator();
    private HideOutGenerator hideOutGenerator = new HideOutGenerator();
    private int levelNum { get; set; } = 1;


    [field : SerializeField] Transform levelRootTransform { get; set; }
    public GameObject currentLevelLand { get; set; }
    [SerializeField] Material islandMaterial;

    [Header("Finish",order =1)]
    [SerializeField] FinishView finishView;
    private GameObject finishCircle { get; set; }
    

    [Header("HideOuts", order = 2)]
    [SerializeField] private List<HideOutView> hideOutModels;
    [SerializeField] int hideOutCount = 0;
    [SerializeField, Range(0.3f, 1f)] float hideOutPlacementJitterness = 0;


    [Header("Island", order = 0)]
    [SerializeField] float defaultMaxRadius = 0;
    [SerializeField] float defaultMinRadius = 0;
    [HideInInspector] private float maxRadius;
    [HideInInspector] private float minRadius;
    [SerializeField, Range(0, 5)] private float islandAltitude;
    [SerializeField, Range(0, 20)] private float islandDepth;
    [SerializeField] private float spawnSetbackFromBoat;

    public Vector3 playerEntry { get; private set; }
    public Vector3 playerExit { get; private set; }

    public Action onExitTrigger { get;  set; }
    public Action onNewLevel { get; set; }

    protected override void Awake()
    {
        base.Awake();
        maxRadius = defaultMaxRadius + levelNum;
        minRadius = defaultMinRadius + levelNum;
        islandDepth = (islandDepth<islandAltitude)? islandAltitude : islandDepth;
    }
    private void Start()
    {
        landGenerator.onEntryExitGeneration += reactOnEntryExitGeneration;
        PlayerService.instance.onReachingLand += reactOnPlayerInIsland_Player;
        BoatService.instance.onAreaPassed += reactOnAreaPassed_Boat;
        BoatService.instance.onBoatInNewLevel += reactOnBoatInNewLevel;
    }

    #region re-Actions
    private void reactOnBoatInNewLevel(Vector3 boatPosition)
    {
        Vector3 newIslandPosition = boatPosition + Vector3.forward * (maxRadius + spawnSetbackFromBoat) + Vector3.up * islandAltitude;
        createNewLevel(newIslandPosition);
    }

    private void reactOnAreaPassed_Boat()
    {
        clearWorld();
        ++levelNum;
        onNewLevel?.Invoke();
    }
    public void reloadCurrentLevel()
    {
        clearWorld();
        onNewLevel?.Invoke();
    }

    private void reactOnPlayerInIsland_Player()
    {
        spwanLevelExit();
    }
    private void reactOnEntryExitGeneration(Vector3 entry, Vector3 exit)
    {
        playerEntry = entry;
        playerExit = exit;
        //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //sphere.transform.position = entry;
        //Instantiate(sphere, exit, Quaternion.identity);
    } 
    #endregion

    private void spwanLevelExit()
    {
        if (finishCircle == null)
            finishCircle = Instantiate<FinishView>(finishView, playerExit, Quaternion.identity, levelRootTransform).gameObject;
        else
        {
            finishCircle.transform.position = playerExit;
            finishCircle.SetActive(true);
        }
    }

    public void createCurrLevel(Vector3 position)
    {
        currentLevelLand = landGenerator.createLand(position, islandMaterial, minRadius, maxRadius, -islandDepth, 10);
        currentLevelLand.transform.parent = levelRootTransform;
        //StartCoroutine(hideOutGenerator.createEnemyHideouts(currentLevelLand, 15,hideOutModels));
        StartCoroutine(hideOutGenerator.createInCircularPat(currentLevelLand, hideOutCount,hideOutModels, minRadius * hideOutPlacementJitterness, minRadius));
    }

    public void createNewLevel(Vector3 position)
    {
        createCurrLevel(position);
    }

    public List<Vector3> getAllRoofPos()
    {
        return hideOutGenerator.getHideOutRoofs();
    }

    public List<Transform> getAllHideOutTransform()
    {
        return hideOutGenerator.getHideOutTransform();
    }

    public Vector3 getBoatExit()
    {
        Vector3 waterLoc = playerExit + Vector3.forward * 10f;
        return new Vector3(waterLoc.x, 0f, waterLoc.z);
    }

    private void clearWorld()
    {
        GameObject land = currentLevelLand;
        Destroy(land);
        hideOutGenerator.removeAllHideOuts();
        requestEnemyClearance();
    }

    private void requestEnemyClearance()
    {
        EnemyService.instance.destroyAllEnemies();
    }

    public int getLevelNum()
    {
        return levelNum;
    }
    private void OnDisable()
    {
        if (landGenerator != null)
        {
            landGenerator.onEntryExitGeneration -= reactOnEntryExitGeneration;
        }
        PlayerService.instance.onReachingLand -= reactOnPlayerInIsland_Player;
        BoatService.instance.onAreaPassed -= reactOnAreaPassed_Boat;
        BoatService.instance.onBoatInNewLevel -= reactOnBoatInNewLevel;

    }
}
