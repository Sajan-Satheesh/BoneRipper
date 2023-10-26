using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuadraticCurve : MonoBehaviour
{
    [SerializeField] Transform start = null;
    [SerializeField] Transform end = null;
    [SerializeField] Transform control = null;
    [SerializeField] float divisionScale;
    [SerializeField] List<Vector3> path = new List<Vector3>();
    [SerializeField] List<Vector3> recordedPath = new List<Vector3>();
    [SerializeField] MeshGenerator world;
    public Action<Vector3> initEndPosition;
    private bool record = false;

    private void Awake()
    {
        initEndPosition = setEndPosition;
    }

    private void setEndPosition(Vector3 endPos)
    {
        end.position = endPos;
    }

    public Vector3 evaluate(float t)
    {
        Vector3 startToControl = Vector3.Lerp(start.position, control.position, t);
        Vector3 conrolToEnd = Vector3.Lerp(control.position, end.position, t);
        return Vector3.Lerp(startToControl, conrolToEnd, t);
    }
    private void OnDrawGizmos()
    {
        control.position = Vector3.Lerp(start.position, end.position, 0.5f);
        float controlPositionOffset = end.position.y > start.position.y ? end.position.y + 3f : start.position.y + 3f;
        control.position = new Vector3(control.position.x, controlPositionOffset, control.position.z);


        if (start == null || end == null || control == null) return;
        int betweenPoints = (int)(Vector3.Distance(start.position, end.position)*divisionScale);
        path.Clear();
        for(int i=0; i < betweenPoints; i++)
        {
            Vector3 position = evaluate(i / (float)betweenPoints);
            Gizmos.DrawSphere( position, 0.2f);
            path.Add(position);
        }
        
    }

    public List<Vector3> recordPath()
    {
        recordedPath.Clear();
        recordedPath = path.ToList<Vector3>();
        return recordedPath;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            recordPath();
        }
    }
}
