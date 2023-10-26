using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject player;
    Vector3 defaultposition;

    private void Awake()
    {
        defaultposition = transform.position - player.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        UpdateCamLocation();

    }

    private void UpdateCamLocation()
    {
        if (player != null)
        {
            transform.position = player.transform.position + defaultposition;
        }
    }

    private void FixedUpdate()
    {
        
        Vector3 playerDirection = (player.transform.position - transform.position).normalized;
        float distanceBetwPlayer = Vector3.Distance(player.transform.position, transform.position);
        Debug.DrawRay(transform.position, playerDirection*distanceBetwPlayer, Color.yellow);
        RaycastHit hitObstruction;
        Color hitMaterial;
        if (Physics.Raycast(gameObject.transform.position, playerDirection, out hitObstruction, distanceBetwPlayer))
        {
            hitObstruction.collider.gameObject.GetComponent<MeshRenderer>().material.shader.FindPropertyIndex("transparency").Equals(0.5f);
        }
    }
}
