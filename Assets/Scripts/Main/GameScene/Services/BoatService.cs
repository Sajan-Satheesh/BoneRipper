
using System;
using System.ComponentModel;
using UnityEngine;

public class BoatService : GenericSingleton<BoatService>
{
    private BoatController boatController;
    [SerializeField] private SO_Boat boatSpecs;
    [SerializeField] private float islandSpawnSetback;
    [SerializeField] private float islandSpawnheight;
    [field: SerializeField] public Transform boatRootTransform { get; private set; }


    private void Start()
    {
        initializeBoat();
        WorldService.instance.createCurrLevel(boatController.getBoatPosition() + Vector3.forward * (WorldService.instance.islandTotalRadius + islandSpawnSetback) + Vector3.up * islandSpawnheight);
        PlayerService.instance.initializePlayer(boatController.getBoatPosition());
    }

    void initializeBoat()
    {
        if (boatController != null) return;

        boatController = new BoatController(boatSpecs);

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
