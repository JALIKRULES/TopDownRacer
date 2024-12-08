using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNode : MonoBehaviour
{
    [Header("This is the waypoint we are going towards, not yet reached")]
    public float minDistanceToReachWaypoint = 5;

    public WaypointNode[] nextWaypointNode;

    public WaypointNode[] ReturnNextWaypoints()
    {
        return nextWaypointNode;
    }

    public Vector2 GetSpecificWaypointPosition(int index)
    {
        return nextWaypointNode[index].transform.position;
    }


}
