
using System.Collections.Generic;
using UnityEngine;

public class BoatController
{
    private BoatModel boatModel;
    private BoatView boatView;
    public BoatController(SO_Boat boatDetails)
    {
        boatView = Object.Instantiate(boatDetails.boatView,BoatService.instance.boatRootTransform);
        boatView.getBoatController(this);
        boatModel = new BoatModel(boatDetails, boatView);
        
    }

    #region boat state functions
    public void onUpdate()
    {
        runStateMachine();
    }

    void runStateMachine()
    {
        switch (boatModel.boatMovementState)
        {
            case BoatMovementStates.stationary:
                spawnBoat(boatModel.spawnPosition);
                break;
            case BoatMovementStates.moving:
                moveBoat();
                break;
            case BoatMovementStates.sinking:
                sink();
                break;
            case BoatMovementStates.hidden:
                hideBoat();
                break;
            case BoatMovementStates.withPlayer:
                break;
            default:
                break;
        }
    }
    #endregion

    public BoatMovementStates getBoatState()
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
    private void setState(BoatMovementStates state)
    {
        boatModel.boatMovementState = state;
    }

    private void moveBoat()
    {
        rowBoatForward();
        detectIslandAhead(boatModel.islandDetectionDistance);
    }
    private void rowBoatForward()
    {
        boatModel.currentSpeed = boatModel.defaultSpeed * Time.deltaTime;
        boatModel.boat.transform.Translate(boatModel.boat.transform.forward * boatModel.currentSpeed);
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
                setState(BoatMovementStates.sinking);
            }
            
            if (boatModel.rayHit_IslandDetector.distance < 0.6f)
                boatModel.isBoatHit = true;
        }
        if (boatModel.isBoatHit)
        {
            //PlayerService.instance.readyToJump = false;
            setState(BoatMovementStates.sinking);
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
            setState(BoatMovementStates.hidden);
        }
    }

    private void resetBoatParams()
    {
        boatModel.isBoatVisible = true;
        boatModel.isBoatHit = false;
        boatModel.isBoatSafe = true;
    }

    private void spawnBoat(Vector3 spawnPosition)
    {
        boatModel.boat.transform.position = spawnPosition;
        unhideBoat();
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