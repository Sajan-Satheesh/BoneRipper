using UnityEngine;

public class FinishMarkerView : MonoBehaviour
{
    [SerializeField] LayerMask collidableLayers;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out PlayerView _))
        {
            WorldService.instance.events.InvokeOnExitTrigger();
            gameObject.SetActive(false);
        } 
    }

}
