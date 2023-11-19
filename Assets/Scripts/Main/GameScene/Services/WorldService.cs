using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldService : GenericSingleton<WorldService>
{
    private LandGenerator landGenerator = new LandGenerator();
    private HideOutGenerator hideOutGenerator = new HideOutGenerator();
    [field : SerializeField] Transform levelRootTransform { get; set; }
    [SerializeField] GameObject currentLevelLand;
    [SerializeField] List<GameObject> hideOutModels;
    [SerializeField] int levelNum = 0;
    [SerializeField] int islandRadius = 0;
    public int islandTotalRadius;
    [SerializeField] float islandDepth;
    [SerializeField] Material islandMaterial;
    
    public Vector3 playerEntry { get; private set; }
    public Vector3 playerExit { get; private set; }

    //weapon spear test
    private CurveGenerator curveGenerator = new CurveGenerator();
    [SerializeField] Vector3 targetEnemy;
    [SerializeField] Vector3 accesibleEnemy;
    [SerializeField] List<Vector3> roofTops;
    [SerializeField] Transform spearWeapon;
    [SerializeField] GameObject pathElement;


    protected override void Awake()
    {
        base.Awake();
        islandTotalRadius = islandRadius + levelNum;
    }
    private void OnEnable()
    {
        landGenerator.setEntryExit += getEntryExit;
    }
    public void createCurrLevel(Vector3 position)
    {
        GameObject land = landGenerator.createLand(position, islandMaterial, islandTotalRadius, -islandDepth, 10);
        land.transform.parent = levelRootTransform;
        StartCoroutine(hideOutGenerator.createEnemyHideouts(land,15,hideOutModels));
    }

    public void createNewLevel(Vector3 position)
    {
        levelNum++;
        createCurrLevel(position);
    }

    private void getEntryExit(Vector3 entry, Vector3 exit)
    {
        playerEntry = entry;
        playerExit = exit;
        Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), entry, Quaternion.identity);
        Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), exit, Quaternion.identity);
    }

    private void OnDisable()
    {
        if (landGenerator != null)
        {
            landGenerator.setEntryExit -= getEntryExit;
        }
        
    }

    private void Update()
    {
        if (roofTops.Count == 0) return;
        if (Vector3.Distance(spearWeapon.position, PlayerService.instance.getPlayerLocation()) < 10f)
        {
            checkNearestEnemy();
            updateCurve();
        }
    }

    private void checkNearestEnemy()
    {
        Vector3 dirFromPlayer = (spearWeapon.position - PlayerService.instance.getPlayerLocation()).normalized;
        foreach(Vector3 pos in roofTops)
        {
            Vector3 dirFromWeapon = (pos - spearWeapon.position).normalized;
            Vector3 dirFromWeaponToAcc = (accesibleEnemy - spearWeapon.position).normalized;
            if (Vector3.Dot(dirFromPlayer, dirFromWeapon) >= Vector3.Dot(dirFromPlayer, dirFromWeaponToAcc))
                accesibleEnemy = pos;
        }
    }

    private void updateCurve()
    {
        if(accesibleEnemy == targetEnemy) return;

        targetEnemy = accesibleEnemy;
        curveGenerator.createPath(spearWeapon.position, targetEnemy, 4f , pathElement);
    }

    internal void getAllRoofPos()
    {
        roofTops = hideOutGenerator.getHideOutRoofs();
    }
}
