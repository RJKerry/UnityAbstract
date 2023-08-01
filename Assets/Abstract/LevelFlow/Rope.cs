using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour //, ICanBeGrabbed
{
    public Transform playerTransform;
    public int ropeSegmentCount = 10;
    public float ropeWidth = 0.1f;
    public float ropeSlack = 0.1f;

    private LineRenderer lineRenderer;
    private List<Vector3> ropePoints;

    private void Start()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;
        lineRenderer.positionCount = ropeSegmentCount;

        ropePoints = new List<Vector3>();
        for (int i = 0; i < ropeSegmentCount; i++)
        {
            ropePoints.Add(Vector3.zero);
        }
    }

/*    public void OnGrabbed(Vector3 hitPoint)
    {
        Debug.Log("Test");

        UpdateRopePoints(hitPoint);
    }*/

    private void UpdateRopePoints(Vector3 startPoint)
    {
        ropePoints[0] = startPoint;
        Vector3 playerPos = playerTransform.position;
        Vector3 objectPos = transform.position;
        Vector3 offset = new Vector3(0, -0.5f, 1);
        playerPos = playerPos + offset;
        Vector3 dir = (playerPos - startPoint).normalized;
        float segmentLength = Vector3.Distance(startPoint, playerPos) / ropeSegmentCount;

        for (int i = 1; i < ropeSegmentCount; i++)
        {
            ropePoints[i] = startPoint + dir * (segmentLength * i);
        }

        // Update the position of the grabbable object in the rope points
        int objectIndex = ropeSegmentCount - 1;
        ropePoints[objectIndex] = objectPos;

        lineRenderer.SetPositions(ropePoints.ToArray());
    }

}