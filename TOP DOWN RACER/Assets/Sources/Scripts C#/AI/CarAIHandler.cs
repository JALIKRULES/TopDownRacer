using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CarAIHandler : MonoBehaviour
{
    public enum AIMode { followPlayer, followWaypoints };

    [Header("AI settings")]
    public AIMode aiMode;
    public float maxSpeed = 16f;
    public bool isAvoidingCars = true;
    [Range(0f, 1.0f)]
    public float skillLevel = 1.0f;

    Vector3 targetPosition = Vector3.zero;
    Transform targetTransform = null;
    float originalMaximumSpeed = 0.0f;
    

    Vector2 avoidanceVectorLerped = Vector2.zero;

    WaypointNode currentWaypoint = null;
    WaypointNode previousWaypoint = null;
    WaypointNode[] allWaypoints;

    PolygonCollider2D polygonCollider2D;

    TopDownCarController topDownCarController;

    void Awake()
    {
        topDownCarController = GetComponent<TopDownCarController>();
        allWaypoints = FindObjectsOfType<WaypointNode>();

        polygonCollider2D = GetComponentInChildren<PolygonCollider2D>();

        originalMaximumSpeed = maxSpeed;
    }

    private void Start()
    {
        SetMaxSpeedBasedONSkillLevel(maxSpeed);
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
        {
            currentWaypoint = FindClosestWaypoint();
            previousWaypoint = currentWaypoint;
        }


        if (currentWaypoint != null)
        {
            targetPosition = currentWaypoint.transform.position;

            float distanceToWaypoint = (targetPosition - transform.position).magnitude;

            if (distanceToWaypoint > 20)
            {
                Vector3 nearestPointOnTheWaypointLine = FindNearestPointOnLine(previousWaypoint.transform.position, currentWaypoint.transform.position, transform.position);

                float segments = distanceToWaypoint / 20.0f;

                targetPosition = (targetPosition + nearestPointOnTheWaypointLine * segments) / (segments + 1);

                Debug.DrawLine(transform.position, targetPosition, Color.cyan);
            }

            if (distanceToWaypoint <= currentWaypoint.minDistanceToReachWaypoint)
            {
                if (currentWaypoint.maxSpeed > 0)
                    SetMaxSpeedBasedONSkillLevel(currentWaypoint.maxSpeed);
                else SetMaxSpeedBasedONSkillLevel(1000);

                previousWaypoint = currentWaypoint;

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

        if (isAvoidingCars)
            AvoidCars(vectorToTarget, out vectorToTarget);

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

        float reduceSpeedDueToCornering = Mathf.Abs(inputX) / 1.0f;

        return 1.05f - reduceSpeedDueToCornering * skillLevel;
    }

    void SetMaxSpeedBasedONSkillLevel(float newSpeed)
    {
        maxSpeed = Mathf.Clamp(newSpeed , 0 , originalMaximumSpeed);

        float skillBAseMaximumSpeed = Mathf.Clamp(skillLevel, 0.3f, 1.0f);
        maxSpeed = maxSpeed * skillBAseMaximumSpeed;
    }

    Vector2 FindNearestPointOnLine(Vector2 lineStartPosition, Vector2 lineEndPosition, Vector2 point)
    {
        Vector2 lineHeadingVector = (lineEndPosition - lineStartPosition);

        float maxDistance = lineHeadingVector.magnitude;
        lineHeadingVector.Normalize();

        Vector2 lineVectorStartToPosition = point - lineStartPosition;
        float dotProduct = Vector2.Dot(lineVectorStartToPosition, lineHeadingVector);

        dotProduct = Mathf.Clamp(dotProduct, 0f, maxDistance);

        return lineStartPosition + lineHeadingVector * dotProduct;
    }

    bool IsCarInFrontOfAICar(out Vector3 position, out Vector3 otherCarRightVector)
    {
        polygonCollider2D.enabled = false;

        RaycastHit2D raycastHit2D = Physics2D.CircleCast(transform.position + transform.up * 0.5f, 1.2f, transform.up, 12, 1 << LayerMask.NameToLayer("Car"));

        polygonCollider2D.enabled = true;

        if (raycastHit2D.collider != null)
        {
            Debug.DrawRay(transform.position, transform.up * 12, Color.red);

            position = raycastHit2D.collider.transform.position;
            otherCarRightVector = raycastHit2D.collider.transform.right;

            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.up * 12, Color.black);

        }

        position = Vector3.zero;
        otherCarRightVector = Vector3.zero;

        return false;
    }

    void AvoidCars(Vector2 vectorToTarget, out Vector2 newVectorToTarget)
    {
        if (IsCarInFrontOfAICar(out Vector3 otherCarPosition, out Vector3 otherCarRightVector))
        {
            Vector2 avoidanceVector = Vector2.zero;

            avoidanceVector = Vector2.Reflect((otherCarPosition - transform.position).normalized, otherCarRightVector - transform.position);

            float distanceToTarget = (targetPosition - transform.position).magnitude;

            float driveToTargetInfluence = 6.0f / distanceToTarget;

            driveToTargetInfluence = Mathf.Clamp(driveToTargetInfluence, 0.30f, 1.0f);

            float avoidanceInfluence = 1.0f - driveToTargetInfluence;

            avoidanceVectorLerped = Vector2.Lerp(avoidanceVectorLerped, avoidanceVector, Time.fixedDeltaTime * 4);

            newVectorToTarget = vectorToTarget * driveToTargetInfluence + avoidanceVectorLerped * avoidanceInfluence;
            newVectorToTarget.Normalize();

            Debug.DrawRay(transform.position, avoidanceVector * 10, Color.green);

            Debug.DrawRay(transform.position, newVectorToTarget * 10, Color.yellow);

            return;
        }

        newVectorToTarget = vectorToTarget;

    }

}
