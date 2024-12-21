using System.Linq;
using UnityEngine;

public class CarAIHandler : MonoBehaviour
{
    public enum AIMode { followPlayer, followWaypoints };

    [Header("AI settings")]
    public AIMode aiMode;
    public float maxSpeed = 16f;


    Vector3 targetPosition = Vector3.zero;
    Transform targetTransform = null;

    WaypointNode currentWaypoint = null;
    WaypointNode[] allWaypoints;

    TopDownCarController topDownCarController;

    void Awake()
    {
        topDownCarController = GetComponent<TopDownCarController>();
        allWaypoints = FindObjectsOfType<WaypointNode>();
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = Vector2.zero;
        switch (aiMode)
        {
            case AIMode.followPlayer: FollowPlayer(); break;
            case AIMode.followWaypoints: FollowWaypoints(); break;
        }

        inputVector.x = TurnTowardsTarget();
        inputVector.y = ApplyThrottleOrBrake(inputVector.x);

        topDownCarController.SetInputVector(inputVector);
    }


    void FollowPlayer()
    {
        if (targetTransform == null)
            targetTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (targetTransform != null)
            targetPosition = targetTransform.position;
    }

    void FollowWaypoints()
    {
        if (currentWaypoint == null)
            currentWaypoint = FindClosestWaypoint();

        if (currentWaypoint != null)
        {
            targetPosition = currentWaypoint.transform.position;

            float distanceToWaypoint = (targetPosition - transform.position).magnitude;

            if (distanceToWaypoint <= currentWaypoint.minDistanceToReachWaypoint)
            {
                if (currentWaypoint.maxSpeed > 0)
                    maxSpeed = currentWaypoint.maxSpeed;
                else maxSpeed = 1000f;

                currentWaypoint = currentWaypoint.nextWaypointNode[Random.Range(0, currentWaypoint.nextWaypointNode.Length)];
            }
        }

    }

    WaypointNode FindClosestWaypoint()
    {
        return allWaypoints
            .OrderBy(t => Vector3.Distance(transform.position, t.transform.position))
            .FirstOrDefault();
    }

    float TurnTowardsTarget()
    {
        Vector2 vectorToTarget = targetPosition - transform.position;
        vectorToTarget.Normalize();

        float angleToTarget = Vector2.SignedAngle(transform.up, vectorToTarget);
        angleToTarget *= -1;

        float steerAmount = angleToTarget / 45.0f;

        steerAmount = Mathf.Clamp(steerAmount, -1.0f, 1.0f);

        return steerAmount;
    }

    float ApplyThrottleOrBrake(float inputX)
    {
        if (topDownCarController.GetVelocityMagnitude() > maxSpeed)
            return 0;

        return 1.05f - Mathf.Abs(inputX) / 1.0f;
    }
}
