
using System;
using System.ComponentModel;
using UnityEngine;

public class BoatService : GenericSingleton<BoatService>
{
    private BoatController boatController;
    [SerializeField] private BoatData boatSpecs;
    
    [SerializeField] public bool isMovingTowards = true;
    [field: SerializeField] public Transform boatRootTransform { get; private set; }
    public EventsBoat events = new EventsBoat();

    private void Start()
    {
        WorldService.instance.events.Subscribe_OnExitTrigger(reactOnExitTrigger_World);
        PlayerService.instance.events.Subscribe_OnReachingBoat(reactOnReachingBoat);
        WorldService.instance.events.Subscribe_OnNewLevel(reactOnNewLevel_World);
        reactOnNewLevel_World();
    }

    #region Requests
    private void requestPlayerInitialization()
    {
        PlayerService.instance.initializePlayer(boatController.getBoatPosition());
    }
    #endregion

    #region re-Actions
    private void reactOnNewLevel_World()
    {
        isMovingTowards = true;
        initializeBoat(Vector3.zero);
        requestPlayerInitialization();
        events.InvokeOnBoatInNewLevel(boatController.getBoatPosition());
    }

    private void reactOnReachingBoat()
    {
        boatController.setState(EnumBoatMovementStates.MOVE_AWAY);
    }

    private void reactOnExitTrigger_World()
    {
        initializeBoat(WorldService.instance.getBoatExit());
    } 
    #endregion

    void initializeBoat(Vector3 position)
    {
        
        if (boatController != null)
        {
            boatController.setState(EnumBoatMovementStates.STATIONARY);
            boatController.spawnBoat(position);
            boatController.initializeDefaultBoatController();
        }
        else
        {
            boatController = new BoatController(boatSpecs);
        }
        
    }
    public Vector3 getBoatPos()
    {
        return boatController.getBoatPosition();
    }
    public Transform getBoatSeatTransform()
    {
        return boatController.getBoatSeatTransform();
    }
    private void OnDisable()
    {
        WorldService.instance.events.UnSubscribe_OnExitTrigger(reactOnExitTrigger_World);
        PlayerService.instance.events.UnSubscribe_OnReachingBoat(reactOnReachingBoat);
        WorldService.instance.events.UnSubscribe_OnNewLevel(reactOnNewLevel_World);
    }
}

// boatController.getBoatPosition() + Vector3.forward* 15f + Vector3.up*3f // island Spawn Position
// Vector3.zero // island test Spawn Position
