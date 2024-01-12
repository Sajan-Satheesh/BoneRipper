
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoatController
{
    private BoatModel boatModel;
    private BoatView boatView;
    public BoatController(BoatData boatDetails)
    {
        boatView = Object.Instantiate(boatDetails.boatView,BoatService.instance.boatRootTransform);
        boatView.getBoatController(this);
        boatModel = new BoatModel(boatDetails, boatView);
        
    }

    public void initializeDefaultBoatController()
    {
        boatModel.isBoatSafe = true;
        boatModel.isBoatHit = false;
        boatModel.isBoatVisible = true;
        //  setState(BoatMovementStates.spawn);
    }

    #region boat state functions
    public void onUpdate()
    {
        runStateMachine();
    }

    public void setState(EnumBoatMovementStates state)
    {
        boatModel.boatMovementState = state;
    }
    void runStateMachine()
    {
        switch (boatModel.boatMovementState)
        {
            case EnumBoatMovementStates.STATIONARY:
                break;
            case EnumBoatMovementStates.MOVE_TOWARDS:
                moveBoat();
                break;
            case EnumBoatMovementStates.SINKING:
                sink();
                break;
            case EnumBoatMovementStates.HIDDEN:
                hideBoat();
                break;
            case EnumBoatMovementStates.MOVE_AWAY:
                moveAway();
                break;
            default:
                break;
        }
    }

    
    #endregion

    public EnumBoatMovementStates getBoatState()
    {
        return boatModel.boatMovementState;
    }
    public void setSpwanPosition(Vector3 position)
    {
        boatModel.spawnPosition = position;
    }

    public Vector3 getBoatPosition()
    {
        return boatModel.boat.transform.position;
    }

    public Transform getBoatSeatTransform()
    {
        return boatModel.boatSeat.transform;
    }
    

    private void moveAway()
    {
        rowBoatForward();
        trackIslandBehind();

    }

    private void trackIslandBehind()
    {
        boatModel.ray_IslandDetector.origin = boatModel.boatObj.transform.position + -boatModel.boatObj.transform.forward * boatModel.boatObj.GetComponent<MeshRenderer>().bounds.size.z / 2;
        boatModel.ray_IslandDetector.direction = -boatModel.boatObj.transform.forward;
        Debug.DrawLine(boatModel.ray_IslandDetector.origin, boatModel.ray_IslandDetector.origin + boatModel.ray_IslandDetector.direction * Mathf.Infinity, Color.black);
        if (Physics.Raycast(boatModel.ray_IslandDetector, out boatModel.rayHit_IslandDetector, Mathf.Infinity, boatModel.mask_IslandDetector))
        {
            if (Vector3.Distance(getBoatPosition(), boatModel.rayHit_IslandDetector.point) > 20f)
            {
                BoatService.instance.events.InvokeOnAreaPassed();
                BoatService.instance.isMovingTowards = true;
            }
        }
    }

    private void moveBoat()
    {
        rowBoatForward();
        detectIslandAhead(boatModel.islandDetectionDistance);
    }
    private void rowBoatForward()
    {
        boatModel.currentSpeed = boatModel.defaultSpeed * Time.deltaTime;
        boatModel.boat.transform.Translate(Vector3.forward * boatModel.currentSpeed);
    }
    private void detectIslandAhead(float distance)
    {
        boatModel.ray_IslandDetector.origin = boatModel.boatObj.transform.position + boatModel.boatObj.transform.forward * boatModel.boatObj.GetComponent<MeshRenderer>().bounds.size.z/2;
        boatModel.ray_IslandDetector.direction = boatModel.boatObj.transform.forward;
        Debug.DrawLine(boatModel.ray_IslandDetector.origin, boatModel.ray_IslandDetector.origin + boatModel.ray_IslandDetector.direction * distance, Color.black);
        if (!boatModel.isBoatHit && Physics.Raycast(boatModel.ray_IslandDetector, out boatModel.rayHit_IslandDetector, distance, boatModel.mask_IslandDetector))
        {
            if (boatModel.isBoatSafe)
            {
                Debug.Log("Press J to Jump");
                PlayerService.instance.readyToJump = true;
                boatModel.isBoatSafe = false;
            }
            else if(PlayerService.instance.readyToJump == false)
            {
                BoatService.instance.isMovingTowards = false;
                setState(EnumBoatMovementStates.SINKING);
            }
            
            if (boatModel.rayHit_IslandDetector.distance < 0.6f)
                boatModel.isBoatHit = true;
        }
        if (boatModel.isBoatHit)
        {
            //PlayerService.instance.readyToJump = false;
            boatModel.isBoatHit = false;
            setState(EnumBoatMovementStates.SINKING);
        }
    }
    private void throwBoat(List<Vector3> positions)
    {
        boatModel.boat.transform.Translate(boatModel.boat.transform.forward * boatModel.currentSpeed);
    }
    private void sink()
    {
        boatModel.currentSpeed = boatModel.defaultSpeed * Time.deltaTime;
        boatModel.boat.transform.Translate(-1 * boatModel.boat.transform.up * boatModel.sinkingSpeed * Time.deltaTime);
        if(boatModel.boat.transform.position.y < -5f)
        {
            setState(EnumBoatMovementStates.HIDDEN);
        }
    }

    private void resetBoatParams()
    {
        boatModel.isBoatVisible = true;
        boatModel.isBoatHit = false;
        boatModel.isBoatSafe = true;
    }

    public void spawnBoat(Vector3 spawnPosition)
    {
        boatModel.boat.transform.position = spawnPosition;
        unhideBoat();
        if (BoatService.instance.isMovingTowards)
        {
            setState(EnumBoatMovementStates.MOVE_TOWARDS);
        }
    }

    private void unhideBoat()
    {
        if (boatModel.isBoatVisible) return;

        resetBoatParams();
        boatModel.boat.SetActive(true);
    }

    private void hideBoat()
    {
        if (!boatModel.isBoatVisible) return;

        boatModel.isBoatVisible = false;
        boatModel.boat.SetActive(false);

    }


    private void destuctBoat()
    {
        boatModel = null;
        Object.Destroy(boatView);
        boatView = null;
    }

    
}