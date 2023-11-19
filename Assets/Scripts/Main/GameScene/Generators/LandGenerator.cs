
using System;
using UnityEngine;



public class LandGenerator 
{
    private int divAngle;
    private int radius;
    private float meshDepth;
    private Material material;

    private const int EXCLUDING_CENTER_VERTICE_INDEX = 1;

    
    private Vector3[] vertices;
    private int[] triangles;
    public Vector3 entry { get; private set; }
    public Vector3 exit { get; private set; }
    public Action<Vector3, Vector3> setEntryExit;

    public GameObject createLand(Vector3 landPosition, Material _mat, int _radius, float _depth, int _divisionAngle)
    {
        material = _mat;
        return createLand(landPosition,_radius,_depth,_divisionAngle);
    }
    public GameObject createLand(Vector3 landPosition, int _radius, float _depth, int _divisionAngle)
    {
        radius = _radius;
        meshDepth = _depth;
        divAngle = _divisionAngle;
        return generateLand(landPosition);
    }


    private GameObject generateLand(Vector3 landPosition)
    {
        
        GameObject generated = new GameObject("land",typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
        createMesh(generated, radius, divAngle);
        generated.transform.position = landPosition;

        int indexOppToEntry = (360 / divAngle * 2);
        //entry = Vector3.Lerp(generated.transform.position, generated.transform.position + vertices[1], 0.9f) + Vector3.up*2;
        //exit = Vector3.Lerp(generated.transform.position, generated.transform.position + vertices[indexOppToEntry], 0.9f) + Vector3.up*2;
        entry += generated.transform.position;
        exit += generated.transform.position;
        setEntryExit(entry, exit);
        return generated;
        //StartCoroutine(createEnemyHideouts(generated));
        //visulaizeVerices();

    }
    private GameObject createMesh(GameObject generated,int maxDistance, int angle)
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

    private void setMeshCollider(GameObject generated)
    {
        MeshCollider collider = generated.GetComponent<MeshCollider>();
        collider.sharedMesh = generated.GetComponent<MeshFilter>().mesh;
        collider.convex = false;
    }
    private void setMeshRenderer(GameObject generated)
    {
        generated.GetComponent<MeshRenderer>().material = material;
    }

}