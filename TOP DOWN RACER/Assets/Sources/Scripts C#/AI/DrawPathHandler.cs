using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPathHandler : MonoBehaviour
{
    public Transform transformRootObject;

    private void OnDrawGizmos()
    {
        if (transformRootObject == null)
            return;

        // Get all the waypoints (children of the rootObject)
        WaypointNode[] waypointNodes = transformRootObject.GetComponentsInChildren<WaypointNode>();

        // Iterate over the waypoint nodes and draw lines between their transforms
        foreach (var waypointNode in waypointNodes)
        {
            // Connect the waypoints in the root object (parent to parent)
            if (waypointNode.transform != transformRootObject)
            {
                Gizmos.color = Color.red;  // You can change the color of the line
                Gizmos.DrawLine(waypointNode.transform.position, waypointNode.transform.position);
            }

            // Iterate over the array of transforms in each waypoint node and connect them
            for (int i = 0; i < waypointNode.ReturnNextWaypoints().Length - 1; i++)
            {
                Gizmos.color = Color.blue;  // Color for connections between array elements
                Gizmos.DrawLine(waypointNode.GetSpecificWaypointPosition(i), waypointNode.GetSpecificWaypointPosition(i + 1));
            }

            // Optionally, connect each transform in the waypoint node to each other
            //foreach (Transform child in waypointNode.GetEachWaypointPosition())
            //{
            //    Gizmos.color = Color.green;  // Color for connections to children
            //    Gizmos.DrawLine(waypointNode.transform.position, child.position);
            //}
        }
    }
}
