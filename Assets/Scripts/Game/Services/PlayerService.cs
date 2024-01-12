using System;
using System.Collections.Generic;
using UnityEngine;


public class PlayerService : GenericSingleton<PlayerService> 
{
    private PlayerController player = null;
    private CurveGenerator curveGenerator = new CurveGenerator();
    public bool readyToJump { get; set; }
    private bool bonesAcquired { get; set; }

    [SerializeField] private PlayerData playerConfig;
    [SerializeField] private Transform mainPlayerRoot;
    [SerializeField] private GameObject jumpCurvePathElement;
    [SerializeField] private float jumpcurveGap;

    public EventsPlayer events = new EventsPlayer();

    private void Start()
    {
        WorldService.instance.events.Subscribe_OnExitTrigger(reactOnExitTrigger_World);
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
            player.InitializeDefaultPlayer();
        }
        else
        {
            player = new PlayerController(playerConfig, spawnPosition);
            GameService.instance.setPlayerShopSlot(player.GetPlayerSlot());
        }
    }


    private void jumpToIsland()
    {
        curveGenerator.createPath(getPlayerLocation(), requestPlayerEntryPos(), jumpcurveGap, jumpCurvePathElement);
        if (Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0))
        {
            curveGenerator.resetPathElements();
            events.InvokeOnInitialtingJump(curveGenerator.path);
            readyToJump = false;
        }
    }
    public void ReachedExit()
    {
        curveGenerator.createPath(getPlayerLocation(), BoatService.instance.getBoatSeatTransform().position, jumpcurveGap, jumpCurvePathElement,render: false);
        events.InvokeOnInitialtingJump(curveGenerator.path);
    }

    private void destroyPlayer()
    {
        player.DestuctPlayer();
        player = null;
    }
    public void gameOver()
    {
        destroyPlayer();
        events.InvokeOnGameOver();
    }
    
    public bool isPlayerAlive()
    {
        return player != null;
    }

    public Vector3 getPlayerLocation()
    {
        if(player != null)
            return player.GetCurrentLocInWorld();
        else return default;
    }

    public float getPlayerSpeed()
    {
        if (player != null)
            return player.GetCurrentSpeed();
        else return 0;
    }

    public Vector3 getPlayerDirection()
    {
        if (player != null)
            return player.GetForwardDirection();
        else return Vector3.forward;
    }
    public void setPlayerRoot(PlayerController playerC)
    {
        playerC.SetPlayerRoot(mainPlayerRoot);
    }

    public void reachedLand()
    {
        events.InvokeOnReachingLand();
        player.SetNonKinematicPlayer();
    }

    private void reactOnExitTrigger_World()
    {
        player.SetkinematicPlayer();
        player.SetState(EnumPlayerStates.LEVEL_END);
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
        return player.GetDefaultWeaponSpot();
    }

    
    private void OnDisable()
    {
        WorldService.instance.events.UnSubscribe_OnExitTrigger(reactOnExitTrigger_World);
    }

    
}
