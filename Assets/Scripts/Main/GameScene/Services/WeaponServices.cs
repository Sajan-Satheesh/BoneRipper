using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponServices : GenericSingleton<WeaponServices>
{
    private CurveGenerator curveGenerator = new CurveGenerator();
    [SerializeField] public Dictionary<Weapons, ShootablePool> shootables;
    [SerializeField] Bullet bullet;
    [field: SerializeField] arrowView arowWeapon { get; set; }
    [SerializeField] bwView bwWeapon;
    [SerializeField] GameObject bwWeaponGO;
    [SerializeField] Vector3 targetEnemy { get; set; }
    [SerializeField] Vector3 accesibleEnemy;
    [SerializeField] List<Vector3> roofTops;
    [SerializeField] GameObject spearWeapon;
    
    [SerializeField] List<GameObject> arrows { get; set; }
    [SerializeField] GameObject pathElement;
    [SerializeField] int index = 0;
    [SerializeField] int arrowIndex { get; set; } = 0;
    [SerializeField] bool weaponsReady = false;
    [SerializeField] Transform rootTransform;
    [SerializeField] float trailActivationDistance;
    [SerializeField] float spearReachableDistance;

    override protected void Awake()
    {
        base.Awake();
        arrows = new List<GameObject>();
        shootables = new Dictionary<Weapons, ShootablePool>();
        shootables.Add(Weapons.spear, new ShootablePool(arowWeapon));
    }
    private void Start()
    {
        PlayerService.instance.onReachingLand += reactOnPlayerReachingLand;
        WorldService.instance.onExitTrigger += reactOnExitTrigger;
        BoatService.instance.onAreaPassed += reactOnAreaPassed_Boat;
    }

    private void reactOnExitTrigger()
    {
        deSpawnBwWeapon();
    }

    public void spawnBwWeapon(Transform userTransform)
    {
        
        if (bwWeaponGO != null)
        {
            bwWeaponGO.SetActive(true);
        }
        else
        {
            bwWeaponGO = Instantiate(bwWeapon, userTransform).gameObject;
        }
    }

    public void deSpawnBwWeapon()
    {
        bwWeaponGO.SetActive(false) ;
    }
  
    private void reactOnAreaPassed_Boat()
    {
        weaponsReady = false;
    }

    private void reactOnPlayerReachingLand()
    {
        weaponsReady = true;
        SpawnSpear();
        requestRoofTopPos();
        spawnBwWeapon(requestPlayerWeaponPos());

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

    private void requestRoofTopPos()
    {
        roofTops = WorldService.instance.getAllRoofPos();
    }

    private Transform requestPlayerWeaponPos()
    {
        return PlayerService.instance.getDefaultWeaponSpot();
    }
    private void Update()
    {
        if(weaponsReady)
        {
            spearPathManager();
        }
        updateArrows();
    }

    public void shootArrows(Vector3 position, Vector3 direction)
    {
        Shootable shootable = shootables[Weapons.spear].getShootable(position);
        shootable.transform.up = direction.normalized;
    }
    public void returnShootable(Shootable shootable)
    {
        shootables[Weapons.spear].returnShootable(shootable);
    }

    private void updateArrows()
    {
        if (shootables[Weapons.spear].bulletsInUse.Count <= 0) return;
        ++arrowIndex;
        if (arrowIndex >= shootables[Weapons.spear].bulletsInUse.Count) arrowIndex = 0;
        if(shootables[Weapons.spear].bulletsInUse[arrowIndex].transform.position.y < PlayerService.instance.getPlayerLocation().y)
        {
            returnShootable(shootables[Weapons.spear].bulletsInUse[arrowIndex]);
        }
    }

    public void hitDestructable(IDestructable hitObj, Weapons weaponType)
    {
        switch (hitObj.getDestructableType())
        {
            case Destructables.player:
                PlayerService.instance.gameOver();
                break;
            case Destructables.groundEnemy:
                EnemyService.instance.getHitGroundEnemy((GroundEnemyController)hitObj.getController());
                break;
            case Destructables.towerEnemy:
                break;
            default:
                break;
        }
    }


    private void spearPathManager()
    {
        if (roofTops.Count == 0) return;
        if (Vector3.Distance(spearWeapon.transform.position, PlayerService.instance.getPlayerLocation()) < trailActivationDistance)
        {
            checkNearEnemy();
            updateCurve();
        }
        else curveGenerator.resetPathElements();
    }
    
    private void checkNearEnemy()
    {
        ++index;
        if (index >= roofTops.Count) index = 0;
        Vector3 spearPosAt0 = xzPosAt0(spearWeapon.transform.position);
        Vector3 dirFromPlayer = (xzPosAt0(PlayerService.instance.getPlayerLocation()) - spearPosAt0).normalized;
        Vector3 enemyPos = roofTops[index];
        Vector3 dirFromWeapon = (xzPosAt0(enemyPos) - spearPosAt0).normalized;
        Vector3 dirFromWeaponToAcc = (xzPosAt0(accesibleEnemy) - spearPosAt0).normalized;
        if (Vector3.Dot(dirFromWeapon, dirFromPlayer) <= Vector3.Dot(dirFromWeaponToAcc, dirFromPlayer) && Vector3.Distance(spearPosAt0, xzPosAt0(enemyPos)) <= spearReachableDistance)
            accesibleEnemy = enemyPos;
    }

    private Vector3 xzPosAt0(Vector3 pos) { return new Vector3(pos.x, 0f, pos.z); }

    private void updateCurve()
    {
        if (accesibleEnemy == targetEnemy) return;

        targetEnemy = accesibleEnemy;
        curveGenerator.createPath(spearWeapon.transform.position, targetEnemy, 4f, pathElement);
    }

}

