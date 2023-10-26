using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [SerializeField] private Vector2 mousePosition;
    [SerializeField] RaycastHit positionToMove;
    [SerializeField] private float speed;
    //[SerializeField] GameObject mousePositionObject;
    [SerializeField] Camera SceneCamera;
    private float offset = 0f;
    private LayerMask destructible;
    private LayerMask directionDecision;
    public bool playable = false; 

    MeshGenerator meshgenerator;
    private QuadraticCurve qCurve;
    private BoatMock boat;

    Coroutine enemyDetection;
    Coroutine jumping;

    public PlayerModel(SO_Player soPlayer)
    {
        SoPlayer = soPlayer;

    }

    public SO_Player SoPlayer { get; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
