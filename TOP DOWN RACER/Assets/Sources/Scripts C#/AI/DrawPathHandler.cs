using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPathHandler : MonoBehaviour
{
    public Transform transformRootObject;

    void OnDrawGizmos()
    {
        if (transformRootObject == null) return;

        // �������� ��� �������� �������, ������� ����� ��������� WaypointNode
        WaypointNode[] waypointNodes = transformRootObject.GetComponentsInChildren<WaypointNode>();

        Gizmos.color = Color.green;  // ���� �����

        // �������� �� ���� WaypointNode
        foreach (WaypointNode waypointNode in waypointNodes)
        {
            // ���� � ����� WaypointNode ���� ��������� ���� (nextWaypointNode)
            if (waypointNode.nextWaypointNode != null)
            {
                // ������ ����� ����� ���� ����� � ����� ��� ���������� ������
                foreach (var nextNode in waypointNode.nextWaypointNode)
                {
                    // ���������, ��� ��������� ���� ����������
                    if (nextNode != null)
                    {
                        // ������ ����� �� �������� ���� � ���������� ����
                        Gizmos.DrawLine(waypointNode.transform.position, nextNode.transform.position);
                    }
                }
            }
        }
    }
}
