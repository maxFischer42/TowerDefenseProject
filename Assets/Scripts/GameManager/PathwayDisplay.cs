using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathwayDisplay : MonoBehaviour
{
    public List<Vector2> waypoints = new List<Vector2>();
    //public GeneratePointsForCollider pathCollisionGenerator;
    [HideInInspector] public int waypointCount;
    public float waypointWidth = 0.1f;

    public Vector2 GetNextWaypoint(int ind)
    {
        if (ind == 0 || ind == waypointCount) { return waypoints[ind]; }
        if (waypointCount < ind) return Vector2.zero;
        return waypoints[ind + 1];
    }

    void Start()
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>();
        //pathCollisionGenerator.SetPoints(waypoints);
        foreach (Transform child in children)
        {
            //print(child.name);
            if (child == this.transform) continue;
            waypoints.Add(child.position);
        }
        waypointCount = waypoints.Count;
    }

    private void Update()
    {
        Vector2 previousWaypoint = Vector2.zero;
        int current = 0;
        foreach (Vector2 waypoint in waypoints)
        {            
            if (current == 0 || current == waypointCount)
            {
                previousWaypoint = waypoint;
                current++;
                continue;
            }
            //print("Connecting Node (" + (current - 1) + ") to Node (" + current + ")");
            Debug.DrawLine(previousWaypoint, waypoint);
            previousWaypoint = waypoint;
            current++;
        }        
    }

}
