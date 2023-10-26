using UnityEngine;

[CreateAssetMenu(fileName = "Boat Details", menuName = "Boat", order = 2)]
class SO_Boat : ScriptableObject
{
    public float boatSpeed;
    public BoatView boatView;
}