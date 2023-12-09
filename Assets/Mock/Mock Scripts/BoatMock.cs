using System.Collections;
using UnityEngine;

public class BoatMock : MonoBehaviour
{
    [SerializeField] Transform boatSeat;
    [SerializeField] Transform player;
    [SerializeField] float defaultBoatSpeed;
    [SerializeField ,Range(0,float.MaxValue)] bool activeBoat = true;
    private float boatSpeed;
    Coroutine sinking;
    
    // Start is called before the first frame update
    void Start()
    {
        occupy();
    }


    // Update is called once per frame
    void Update()
    {
        if (activeBoat) moveBoat();
        else sink();
    }
    private void moveBoat()
    {
        boatSpeed = defaultBoatSpeed * Time.deltaTime;
        gameObject.transform.Translate(transform.forward * boatSpeed);
    }
    private void sink()
    {
        gameObject.transform.Translate(-1 * transform.up * 0.2f * Time.deltaTime);
    }

    private void occupy()
    {
        activeBoat = true;
        player.position = boatSeat.position;
        player.gameObject.transform.parent = gameObject.transform;

    }
    public void deOccupy()
    {
        if (activeBoat)
        {
            activeBoat = false;
            player.gameObject.transform.parent = null;
            Mock_Player playerSettings = player.gameObject.GetComponent<Mock_Player>();
            sinking = StartCoroutine(destructBoat());
        }
    }

    

    private IEnumerator destructBoat()
    {
        Color meshColor = gameObject.GetComponent<MeshRenderer>().material.color;
        while (meshColor.a > 0)
        {
            meshColor.a -= 0.1f * Time.deltaTime;
            gameObject.GetComponent<MeshRenderer>().material.color = meshColor;
            yield return null;
        }
        destroyBoat();
    }

    public void spawnBoat(Vector3 spawnPosition)
    {
        gameObject.transform.position = spawnPosition;
        Color meshColor = gameObject.GetComponent<MeshRenderer>().material.color;
        meshColor.a = 1;
        gameObject.GetComponent<MeshRenderer>().material.color = meshColor;
        destroyBoat();
    }

    private bool matchColor(Color a, Color b)
    {
        if (a.a == b.a) return true;
        else return false;
    }
    private void destroyBoat()
    {
        StopCoroutine(sinking);
    }


}
