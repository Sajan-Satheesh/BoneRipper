using System.Collections.Generic;
using UnityEngine;

public class CurveGenerator
{
    public List<Vector3> path { get; private set; } = new List<Vector3>();
    Stack<GameObject> pathElements {get;set;} = new Stack<GameObject>();
    Stack<GameObject> pathElementsInUse{get;set;} = new Stack<GameObject>();

    public void resetPathElements()
    {
        if (pathElementsInUse.Count == 0) return;

        while (pathElementsInUse.Count != 0)
        {
            GameObject element = pathElementsInUse.Pop();
            element.SetActive(false);
            pathElements.Push(element);
        }
    }
    public void createPath(Vector3 start, Vector3 end, float divisionScale, GameObject pathElement, float midHeightOffset = 3f, bool render = true)
    {
        resetPathElements();
        Vector3 control = Vector3.Lerp(start, end, 0.5f);
        float controlPositionOffset = end.y > start.y ? end.y + midHeightOffset : start.y + midHeightOffset;
        control = new Vector3(control.x, controlPositionOffset, control.z);

        int betweenPoints = (int)(Vector3.Distance(start, end) * divisionScale);

        path.Clear();
        for (int i = 0; i < betweenPoints; i++)
        {
            Vector3 position = evaluate(start, end, control, i / (float)betweenPoints);
            if(render) addPathElement(position, pathElement);
            path.Add(position);
        }
    }
    private Vector3 evaluate(Vector3 start, Vector3 end, Vector3 control, float t)
    {
        Vector3 startToControl = Vector3.Lerp(start, control, t);
        Vector3 conrolToEnd = Vector3.Lerp(control, end, t);
        return Vector3.Lerp(startToControl, conrolToEnd, t);
    }


    private void addPathElement(Vector3 position, GameObject element)
    {
        if(pathElements.Count == 0)
        {
            pathElementsInUse.Push(Object.Instantiate(element, position, Quaternion.identity));
        }
        else
        {
            GameObject topElement = pathElements.Pop();
            topElement.transform.position = position;
            topElement.SetActive(true);
            pathElementsInUse.Push(topElement);
        }
    }

}
