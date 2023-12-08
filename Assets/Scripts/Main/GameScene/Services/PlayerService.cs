using System;
using System.Collections.Generic;
using UnityEngine;


public class PlayerService : GenericSingleton<PlayerService> 
{
    private PlayerController player = null;
    private CurveGenerator curveGenerator = new CurveGenerator();
    public bool readyToJump { get; set; }
    private bool bonesAcquired { get; set; }

    [SerializeField] private SO_Player playerConfig;
    [SerializeField] private Transform mainPlayerRoot;
    [SerializeField] private GameObject jumpCurvePathElement;
    [SerializeField] private float jumpcurveGap;

    public Action<List<Vector3>> onInitialtingJump { get; set; }
    public Action onReachingLand { get; set; }
    public Action onReachingBoat { get; set; }
    public Action onGameOver { get; internal set; }

    // Start is called before the first frame update

    private void Start()
    {
        WorldService.instance.onExitTrigger += reactOnExitTrigger_World;
    }

    private void Update()
    {
        if(player!= null)
        {
            if(readyToJump) jumpToIsland();
        }
        
    }
    public void newLevelInitializationSetup()
    {
        readyToJump = false;
        bonesAcquired = false;
        curveGenerator.resetPathElements();
    }
    public void initializePlayer(Vector3 spawnPosition)
    {
        newLevelInitializationSetup();
        if (player != null)
        {
            player.initializeDefaultPlayer();
        }
        else
        {
            player = new PlayerController(playerConfig, spawnPosition);
        }
    }


    private void jumpToIsland()
    {
        curveGenerator.createPath(getPlayerLocation(), requestPlayerEntryPos(), jumpcurveGap, jumpCurvePathElement);
        if (Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0))
        {
            curveGenerator.resetPathElements();
            onInitialtingJump?.Invoke(curveGenerator.path);
            readyToJump = false;
        }
    }
    public void ReachedExit()
    {
        //onReachingExit?.Invoke();
        curveGenerator.createPath(getPlayerLocation(), BoatService.instance.getBoatSeatTransform().position, jumpcurveGap, jumpCurvePathElement,render: false);
        onInitialtingJump?.Invoke(curveGenerator.path);
    }

    private void destroyPlayer()
    {
        player.destuctPlayer();
        player = null;
    }
    public void gameOver()
    {
        destroyPlayer();
        onGameOver?.Invoke();
    }
    

    public Vector3 getPlayerLocation()
    {
        if(player != null)
            return player.getCurrentLocInWorld();
        else return default;
    }

    public float getPlayerSpeed()
    {
        if (player != null)
            return player.getCurrentSpeed();
        else return 0;
    }

    public Vector3 getPlayerDirection()
    {
        if (player != null)
            return player.getForwardDirection();
        else return Vector3.forward;
    }
    internal void setPlayerRoot(PlayerController playerC)
    {
        playerC.setPlayerRoot(mainPlayerRoot);
    }

    internal void reachedLand()
    {
        onReachingLand?.Invoke();
        player.setNonKinematicPlayer();
    }

    private void reactOnExitTrigger_World()
    {
        player.setkinematicPlayer();
        player.setState(PlayerState.levelEnd);
    }

    public Vector3 requestPlayerEntryPos()
    {
        return WorldService.instance.playerEntry;
    }
    public Vector3 requestPlayerExitPos()
    {
        return WorldService.instance.playerExit;
    }

    public Transform getDefaultWeaponSpot()
    {
        return player.getDefaultWeaponSpot();
    }

    
    private void OnDisable()
    {
        WorldService.instance.onExitTrigger -= reactOnExitTrigger_World;
    }

    
}
