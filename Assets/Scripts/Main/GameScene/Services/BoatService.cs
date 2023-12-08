
using System;
using System.ComponentModel;
using UnityEngine;

public class BoatService : GenericSingleton<BoatService>
{
    private BoatController boatController;
    [SerializeField] private SO_Boat boatSpecs;
    
    [SerializeField] public bool isMovingTowards = true;
    [field: SerializeField] public Transform boatRootTransform { get; private set; }
    [field: SerializeField] public Action onAreaPassed { get; set; }
    [field: SerializeField] public Action<Vector3> onBoatInNewLevel { get; set; }

    private void Start()
    {
        WorldService.instance.onExitTrigger += reactOnExitTrigger_World;
        PlayerService.instance.onReachingBoat += reactOnReachingBoat;
        WorldService.instance.onNewLevel += reactOnNewLevel_World;
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
        onBoatInNewLevel?.Invoke(boatController.getBoatPosition());
    }

    private void reactOnReachingBoat()
    {
        boatController.setState(BoatMovementStates.moveAway);
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
            boatController.setState(BoatMovementStates.stationary);
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
}

// boatController.getBoatPosition() + Vector3.forward* 15f + Vector3.up*3f // island Spawn Position
// Vector3.zero // island test Spawn Position
