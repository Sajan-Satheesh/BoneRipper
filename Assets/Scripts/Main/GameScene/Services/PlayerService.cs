using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerService : GenericSingleton<PlayerService> 
{
    public PlayerController player;
    private CurveGenerator curveGenerator = new CurveGenerator();
    internal bool readyToJump = false;

    [SerializeField] private SO_Player playerConfig;
    [SerializeField] private Transform mainPlayerRoot;
    [SerializeField] private GameObject jumpCurvePathElement;
    [SerializeField] private float jumpcurveGap;
    public Action<List<Vector3>> jumpPressed { get; set; }
    // Start is called before the first frame update

    public void initializePlayer(Vector3 spawnPosition)
    {
        player = new PlayerController(playerConfig, spawnPosition);
    }

    private void Update()
    {
        if (readyToJump)
        {
            curveGenerator.createPath(getPlayerLocation(), WorldService.instance.playerEntry, jumpcurveGap, jumpCurvePathElement);
            if (Input.GetKeyDown(KeyCode.J))
            {
                curveGenerator.resetPathElements();
                jumpPressed?.Invoke(curveGenerator.path);
                readyToJump = false;
            }
        } 
    }

    public Vector3 getPlayerLocation()
    {
        if(player != null)
            return player.getCurrentLocInWorld();
        else return Vector3.zero;
    }

    internal void setPlayerRoot(PlayerController playerC)
    {
        playerC.setPlayerRoot(mainPlayerRoot);
    }

}
