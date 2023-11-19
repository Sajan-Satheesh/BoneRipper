using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController instance;
    private void Start()
    {
        if(instance == null) { instance = this; }
        else Destroy(this);
    }

    Vector3 mousePosOnScreen;
    Vector3 mousePosOnWorld;

    public Vector3 getMousePostion()
    {
        mousePosOnScreen = Input.mousePosition;
        return mousePosOnScreen;
    }

    private void Update()
    {
        
    }
}
