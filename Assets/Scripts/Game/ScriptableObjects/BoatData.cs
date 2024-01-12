using UnityEngine;

[CreateAssetMenu(fileName = "Boat Details", menuName = "Boat", order = 2)]
public class BoatData : ScriptableObject
{
    public float boatSpeed;
    public float islandDetectionDistance;
    public BoatView boatView;
    public EnumBoatMovementStates boatMovementState;
    public float sinkSpeed;
    public LayerMask islandMask;
}