using UnityEngine;

[CreateAssetMenu(fileName = "Boat Details", menuName = "Boat", order = 2)]
public class SO_Boat : ScriptableObject
{
    public float boatSpeed;
    public float islandDetectionDistance;
    public BoatView boatView;
    public BoatMovementStates boatMovementState;
    public float sinkSpeed;
    public LayerMask islandMask;
}