
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;



public class MeshGenerator : MonoBehaviour
{
    [SerializeField] Transform objCenter;
    [SerializeField] int divAngle;
    [SerializeField] public int radius;
    [SerializeField] float meshDepth;
    //[SerializeField] float sphereScale;
    [SerializeField] private Material material;
    [SerializeField] private int hideOutCounts;
    [SerializeField] private GameObject hideOut;

    private const int EXCLUDING_CENTER_VERTICE_INDEX = 1;

    
    private Vector3[] vertices;
    private int[] triangles;
    [field:SerializeField] public Vector3 entry { get; private set; }
    public Vector3 exit { get; private set; }

    private void Awake()
    {
        
    }

    public void generateLand(Vector3 landPosition)
    {
        GameObject generated = new GameObject("land",typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
        createMesh(generated, radius, divAngle);
        generated.transform.position = landPosition + Vector3.up * 1f;

        int indexOppToEntry = (360 / divAngle * 2);
        entry = Vector3.Lerp(generated.transform.position, generated.transform.position + vertices[1], 0.9f) + Vector3.up*2;
        exit = Vector3.Lerp(generated.transform.position, generated.transform.position + vertices[indexOppToEntry], 0.9f) + Vector3.up*2;

        //StartCoroutine(createEnemyHideouts(generated));
        //visulaizeVerices();

    }
    public GameObject createMesh(GameObject generated,int maxDistance, int angle)
    {
        Mesh mesh = generated.GetComponent<MeshFilter>().mesh;

        initializeTrisAndVerices(angle);
        int divisions = (360 / angle);
        generateVertices(divisions, angle, maxDistance);
        generateTringles(divisions);
        prepareFinalMesh(ref mesh, generated);
        generated.layer = LayerMask.NameToLayer("Land");
        
        return generated;
    }

    private void prepareFinalMesh(ref Mesh mesh, GameObject generated)
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        setMeshRenderer(generated);
        setMeshCollider(generated);
    }

    private void generateVertices(int divisions, int angle, float maxDistance)
    {
        Vector3 center = Vector3.zero;
        Vector3 entryDirection = Vector3.back;
        vertices[0] = center;

        float offset = 0f;
        int maxIterations = divisions;

        for (int i = 1; i < maxIterations; i++)
        {
            offset = UnityEngine.Random.Range(7, 10) * 0.1f;

            Quaternion axis = Quaternion.AngleAxis(i * angle, Vector3.up);
            Vector3 direction = axis * entryDirection;
            Vector3 maxPostion = center + direction * maxDistance;
            Vector3 placementPostion = Vector3.Lerp(center, maxPostion, offset);
            if (i == 1)
            {
                entry = Vector3.Lerp(center, placementPostion, 0.9f);
            }
            if (i == maxIterations / 2)
            {
                exit = Vector3.Lerp(center, placementPostion, 0.9f);
            }
            vertices[i] = placementPostion;
        }
        for (int i = 1; i < maxIterations; i++)
        {
            vertices[maxIterations + i - 1] = vertices[i];
        }
        for (int i = 1; i < maxIterations; i++)
        {
            vertices[maxIterations * 2 + i - 2] = vertices[i] + new Vector3(0, meshDepth, 0);
        }
    }

    private void generateTringles(int divisions)
    {
        int maxIterations = divisions;
        int triIndex = 0;
        const int V1 = 0;
        const int V2 = 1;
        const int V3 = 2;
        int[] tri = new int[3];
        for (int i = 1; i < divisions; i++)
        {

            tri[V1] = i;
            tri[V2] = i >= divisions - 1 ? 1 : i + 1;
            tri[V3] = 0;

            addIndicesToTris(ref triIndex, tri);
        }
        for (int i = 0; i < divisions - 1; i++)
        {
            tri[V1] = i + divisions;
            tri[V2] = i + divisions * 2 - 1;
            tri[V3] = i + divisions + 1;
            addIndicesToTris(ref triIndex, tri);
            tri[V1] = i + divisions * 2 - 2;
            tri[V2] = i + divisions * 2 - 1;
            tri[V3] = i + divisions;
            addIndicesToTris(ref triIndex, tri);
        }
    }

    private void addIndicesToTris(ref int triIndex, int[] tri)
    {
        triangles[triIndex + 0] = tri[0];
        triangles[triIndex + 1] = tri[1];
        triangles[triIndex + 2] = tri[2];
        triIndex += 3;
    }

    private void initializeTrisAndVerices(int angle)
    {
        int upperVertices = (int)(360 / angle);
        int midVertices = upperVertices - 1;
        int lowerVertices = upperVertices - 1;
        int totalVertices = upperVertices + midVertices + lowerVertices;
        vertices = new Vector3[totalVertices];

        int flatTris = upperVertices - 1;
        int sideTris = lowerVertices * 2;
        int totalTris = flatTris + sideTris;
        triangles = new int[totalTris * 3];
    }

    /*private void visulaizeVerices()
    {
        prepareVisualizersList();
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        foreach (Vector3 vertice in vertices)
        {
            GameObject vSphere = Instantiate(sphere, gameObject.transform);
            vSphere.transform.localPosition = vertice;
            vSphere.transform.localScale *= sphereScale;
            visualizers.Add(vSphere);
        }
        Destroy(sphere);
    }*/

    /*private void prepareVisualizersList()
    {
        foreach (GameObject obj in visualizers)
        {
            Destroy(obj);
        }
        visualizers.Clear();
    }*/

    private void setMeshCollider(GameObject generated)
    {
        generated.GetComponent<MeshCollider>().convex = false;
    }
    private void setMeshRenderer(GameObject generated)
    {
        generated.GetComponent<MeshRenderer>().material = material;
    }

    private IEnumerator createEnemyHideouts(GameObject generated)
    {
        List<GameObject> hideOuts = new List<GameObject>();
        while (hideOuts.Count < hideOutCounts)
        {
            RaycastHit hideOutHit;
            Vector3 randomDirection = generateRandomDirection(out randomDirection);
            Vector3 rayOrigin = generated.transform.position + Vector3.up*0.5f;
            
            if (Physics.Raycast(rayOrigin, randomDirection, out hideOutHit, Mathf.Infinity, LayerMask.GetMask("Land")))
            {
                Debug.DrawLine(rayOrigin, rayOrigin + randomDirection * 20, Color.green, 100f);
                if (possibleSpwan(hideOuts, hideOutHit.point))
                {
                    GameObject hideO = GameObject.Instantiate(hideOut, hideOutHit.point, Quaternion.identity, generated.transform);
                    hideOuts.Add(hideO);
                }
            }
            else Debug.DrawLine(rayOrigin, rayOrigin + randomDirection * 100, Color.red, 1f);

            yield return null;
        }
    }

    private bool possibleSpwan(List<GameObject> hideOuts, Vector3 spawnPosition)
    {
        for (int i = 0; i < hideOuts.Count; i++)
        {
            float distanceBetweenHideOuts = Vector3.Distance(hideOuts[i].transform.position, spawnPosition);
            if (distanceBetweenHideOuts < 3f)
            {
                return false;
            }
        }
        return true;
    }

    private Vector3 generateRandomDirection(out Vector3 randomDirection)
    {
        float randomY = UnityEngine.Random.Range(0f, -0.1f);
        float randomAngle = UnityEngine.Random.Range(0f,360f);
        
        float randomX = Mathf.Cos(randomAngle);
        float randomZ = Mathf.Sin(randomAngle);

        randomDirection = new Vector3(randomX,randomY,randomZ);
        return randomDirection;
    }

}