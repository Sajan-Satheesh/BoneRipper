
using UnityEngine;
class BoatModel
{
    public float defaultSpeed;
    public float currentSpeed;
    public float sinkingSpeed;
    public float islandDetectionDistance;
    public Vector3 spawnPosition;
    public GameObject boat;
    public GameObject boatSeat;
    public GameObject boatObj;
    public BoatMovementStates boatMovementState;
    public Ray ray_IslandDetector;
    public RaycastHit rayHit_IslandDetector;
    public LayerMask mask_IslandDetector;
    public bool isBoatSafe = true;
    public bool isBoatHit = false;
    public bool isBoatVisible = true;
    public BoatModel(SO_Boat boatData, BoatView boatView)
    {
        defaultSpeed = boatData.boatSpeed;
        sinkingSpeed = boatData.sinkSpeed;
        islandDetectionDistance = boatData.islandDetectionDistance;
        boatMovementState = boatData.boatMovementState;
        mask_IslandDetector = boatData.islandMask;
        boat = boatView.gameObject;
        boatObj = boat.transform.GetChild(0).gameObject;
        boatSeat = boat.transform.GetChild(1).gameObject;
        ray_IslandDetector = new Ray(boat.transform.position, boat.transform.forward);
    }
    

}