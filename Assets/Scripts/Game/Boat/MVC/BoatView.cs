using UnityEngine;

public class BoatView : MonoBehaviour
{
    BoatController boatController;
    [SerializeField] EnumBoatMovementStates boatState;

    public void getBoatController(BoatController _boatController)
    {
        if (boatController == null)
        {
            boatController = _boatController;
        }
    }

    private void Update()
    {
        boatController.onUpdate();
        if((int)Time.time % 2 == 0)
            boatState = boatController.getBoatState();
    }


}
