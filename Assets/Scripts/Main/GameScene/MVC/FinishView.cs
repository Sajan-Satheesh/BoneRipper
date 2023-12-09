using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishView : MonoBehaviour
{
    [SerializeField] LayerMask collidableLayers;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out PlayerView ip))
        {
            WorldService.instance.onExitTrigger?.Invoke();
            this.gameObject.SetActive(false);
        } 
    }

}
