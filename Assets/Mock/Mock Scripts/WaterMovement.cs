using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMovement : MonoBehaviour
{
    [SerializeField] Mock_Player boat;
    MeshRenderer thisMeshMaterial;
    [SerializeField] Vector2 reqOffset;

    private void Awake()
    {
        thisMeshMaterial = GetComponent<MeshRenderer>();
        Debug.Log(thisMeshMaterial.material.mainTexture.name);
    }

    // Update is called once per frame
    void Update()
    {
        if(!boat.playable) thisMeshMaterial.material.mainTextureOffset += reqOffset * Time.deltaTime ;
    }
}
