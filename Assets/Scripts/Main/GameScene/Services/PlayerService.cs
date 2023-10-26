using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerService : MonoBehaviour
{
    public static PlayerService instance;

    [SerializeField] public GameObject Player;
    private void Awake()
    {
        if(instance == null)
        {
            instance= this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    

    // Start is called before the first frame update
    void Start()
    {
        
    }


}
