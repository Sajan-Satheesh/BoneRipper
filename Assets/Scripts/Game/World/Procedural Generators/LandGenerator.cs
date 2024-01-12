
using System;
using UnityEditor;
using UnityEngine;



public class LandGenerator 
{
    private int divAngle;
    private float maxRadius;
    private float minRadius;
    private float meshDepth;
    private Material material;

    private const int EXCLUDING_CENTER_VERTICE_INDEX = 1;

    
    private Vector3[] vertices;
    private Vector2[] uvs;
    private int[] triangles;
    public Vector3 entry { get; private set; }
    public Vector3 exit { get; private set; }
    

    public GameObject createLand(Vector3 landPosition, Material _mat, float _minRadius, float _maxRadius, float _depth, int _divisionAngle)
    {
        material = _mat;
        return createLand(landPosition, _minRadius, _maxRadius, _depth,_divisionAngle);
    }
    public GameObject createLand(Vector3 landPosition, float _minRadius, float _maxRadius, float _depth, int _divisionAngle)
    {
        maxRadius = _maxRadius;
        minRadius = _minRadius;
        meshDepth = _depth;
        divAngle = _divisionAngle;
        return generateLand(landPosition);
    }


    private GameObject generateLand(Vector3 landPosition)
    {
        
        GameObject generated = new GameObject("land",typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
        createMesh(generated, divAngle);
        generated.transform.position = landPosition;

        //int indexOppToEntry = (360 / divAngle * 2);
        entry += generated.transform.position;
        exit += generated.transform.position;
        WorldService.instance.events.InvokeOnEntryExitGeneration(entry, exit);
        return generated;


    }
    private GameObject createMesh(GameObject generated, int angle)
    {
        Mesh mesh = generated.GetComponent<MeshFilter>().mesh;

        initializeTrisAndVerices(angle);
        int divisions = (360 / angle);
        generateVertices(divisions, angle);
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
        prepareUVs(ref mesh);
        mesh.RecalculateNormals();
        setMeshRenderer(generated);
        setMeshCollider(generated);
    }

    private void prepareUVs(ref Mesh mesh)
    {
        uvs = new Vector2[vertices.Length];
        Vector2 uvCenter = new Vector2(0.5f, 0.5f);
        for(int i = 0; i < uvs.Length; i++ )
        {
            if (MathF.Abs(vertices[i].y) > 0)
            {
                uvs[i] = uvCenter + new Vector2(vertices[i].x, vertices[i].z)/ (Math.Abs(meshDepth));
            }
            else uvs[i] = uvCenter + new Vector2(vertices[i].x, vertices[i].z)/maxRadius;
        }
        mesh.uv= uvs;
    }

    private void generateVertices(int divisions, int angle)
    {
        Vector3 center = Vector3.zero;
        Vector3 entryDirection = Vector3.back;
        vertices[0] = center;

        //float offset = 0f;
        int maxIterations = divisions;

        for (int i = 1; i < maxIterations; i++)
        {
            //offset = UnityEngine.Random.Range(7, 10) * 0.1f;

            Quaternion axis = Quaternion.AngleAxis(i * angle, Vector3.up);
            Vector3 direction = axis * entryDirection;
            float distance = UnityEngine.Random.Range(minRadius, maxRadius);
            //Vector3 maxPostion = center + direction * maxRadius;
            //Vector3 placementPostion = Vector3.Lerp(center, maxPostion, offset);
            Vector3 placementPostion = center + direction * distance;
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