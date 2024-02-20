using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponServices : GenericSingleton<WeaponServices>
{
    private CurveGenerator curveGenerator = new CurveGenerator();
    [SerializeField] public Dictionary<EnumWeapons, ShootablePool> shootablePoolCollection;
    [SerializeField] ArrowView arowWeapon;
    [SerializeField] BoneWrackerView bwWeapon;
    [SerializeField] bool bwSpawnable;
    private GameObject bwWeaponGO;
    private Vector3 targetEnemy { get; set; }
    private Vector3 accesibleEnemy;
    private List<Vector3> roofTops;
    [SerializeField] GameObject spearWeapon;
    [SerializeField] GameObject pathElement;
    private int index = 0;
    private int arrowIndex = 0;
    [SerializeField] bool weaponsReady = false;
    [SerializeField] Transform rootTransform;
    [SerializeField] float trailActivationDistance;
    [SerializeField] float spearReachableDistance;

    override protected void Awake()
    {
        roofTops = new List<Vector3>();
        base.Awake();
        shootablePoolCollection = new Dictionary<EnumWeapons, ShootablePool>
        {
            { EnumWeapons.SPEAR, new ShootablePool(arowWeapon) }
        };
    }
    private void Start()
    {
        PlayerService.instance.events.Subscribe_OnReachingLand(ReactOnPlayerReachingLand);
        WorldService.instance.events.Subscribe_OnExitTrigger(ReactOnExitTrigger);
        BoatService.instance.events.Subscribe_OnAreaPassed(ReactOnAreaPassed_Boat);
    }

    private void ReactOnExitTrigger()
    {
        DeSpawnBwWeapon();
    }

    public void SpawnBwWeapon(Transform userTransform)
    {
        if (!bwSpawnable) return;
        if (bwWeaponGO != null)
        {
            bwWeaponGO.SetActive(true);
        }
        else
        {
            bwWeaponGO = Instantiate(bwWeapon, userTransform).gameObject;
        }
    }

    private void DeSpawnBwWeapon()
    {
        if(bwWeaponGO != null)
            bwWeaponGO.SetActive(false) ;
    }
  
    private void ReactOnAreaPassed_Boat()
    {
        weaponsReady = false;
    }

    private void ReactOnPlayerReachingLand()
    {
        weaponsReady = true;
        SpawnSpear();
        RequestRoofTopPos();
        SpawnBwWeapon(RequestPlayerWeaponPos());

    }

    private void SpawnSpear()
    {
        if (spearWeapon != null)
        {
            spearWeapon.transform.position = Vector3.Lerp(WorldService.instance.playerEntry, WorldService.instance.playerExit, 0.5f);
            spearWeapon.SetActive(true);
        }
        else
        {
            spearWeapon = Instantiate(spearWeapon, Vector3.Lerp(WorldService.instance.playerEntry, WorldService.instance.playerExit, 0.5f), Quaternion.identity, rootTransform);
        }
    }

    private void RequestRoofTopPos()
    {
        roofTops = WorldService.instance.getAllRoofPos();
    }

    private Transform RequestPlayerWeaponPos()
    {
        return PlayerService.instance.getDefaultWeaponSpot();
    }
    private void Update()
    {
        /*if(weaponsReady)
        {
            SpearPathManager();
        }*/
        UpdateArrowDestruction();
    }

    public void ShootArrows(Vector3 position, Vector3 direction)
    {
        Shootable shootable = shootablePoolCollection[EnumWeapons.SPEAR].getItem(position);
        shootable.transform.up = direction.normalized;
    }
    public void ReturnShootable(Shootable shootable)
    {
        shootablePoolCollection[EnumWeapons.SPEAR].returnItem(shootable);
    }

    private void UpdateArrowDestruction()
    {
        if (shootablePoolCollection[EnumWeapons.SPEAR].inUse.Count <= 0) return;
        ++arrowIndex;
        if (arrowIndex >= shootablePoolCollection[EnumWeapons.SPEAR].inUse.Count) arrowIndex = 0;
        if(shootablePoolCollection[EnumWeapons.SPEAR].inUse[arrowIndex].transform.position.y < PlayerService.instance.getPlayerLocation().y)
        {
            ReturnShootable(shootablePoolCollection[EnumWeapons.SPEAR].inUse[arrowIndex]);
        }
    }

    public void HitDestructable(IDestructable hitObj, EnumWeapons weaponType)
    {
        switch (hitObj.GetDestructableType())
        {
            case EnumDestructables.PLAYER:
                PlayerService.instance.gameOver();
                break;
            case EnumDestructables.GROUND_ENEMY:
                EnemyService.instance.getHitGroundEnemy((GroundEnemyController)hitObj.GetController());
                break;
            case EnumDestructables.TOWER_ENEMY:
                break;
            default:
                break;
        }
    }


    private void SpearPathManager()
    {
        if (roofTops == null) return;
        if (Vector3.Distance(spearWeapon.transform.position, PlayerService.instance.getPlayerLocation()) < trailActivationDistance)
        {
            CheckNearEnemy();
            UpdateCurve();
        }
        else curveGenerator.resetPathElements();
    }
    
    private void CheckNearEnemy()
    {
        ++index;
        if (index >= roofTops.Count) index = 0;
        Vector3 spearPosAt0 = XzPosAt0(spearWeapon.transform.position);
        Vector3 dirFromPlayer = (XzPosAt0(PlayerService.instance.getPlayerLocation()) - spearPosAt0).normalized;
        Vector3 enemyPos = roofTops[index];
        Vector3 dirFromWeapon = (XzPosAt0(enemyPos) - spearPosAt0).normalized;
        Vector3 dirFromWeaponToAcc = (XzPosAt0(accesibleEnemy) - spearPosAt0).normalized;
        if (Vector3.Dot(dirFromWeapon, dirFromPlayer) <= Vector3.Dot(dirFromWeaponToAcc, dirFromPlayer) && Vector3.Distance(spearPosAt0, XzPosAt0(enemyPos)) <= spearReachableDistance)
            accesibleEnemy = enemyPos;
    }

    private Vector3 XzPosAt0(Vector3 pos) { return new Vector3(pos.x, 0f, pos.z); }

    private void UpdateCurve()
    {
        if (accesibleEnemy == targetEnemy) return;

        targetEnemy = accesibleEnemy;
        curveGenerator.createPath(spearWeapon.transform.position, targetEnemy, 4f, pathElement);
    }

}

