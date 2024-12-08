using UnityEngine;

public class CarAIHandler : MonoBehaviour
{
    public enum AIMode { followPlayer, followWaypoints };

    [Header("AI settings")]
    public AIMode aiMode;

    TopDownCarController topDownCarController;

    Vector3 targetPosition = Vector3.zero;
    Transform targetTransform = null;

    void Awake()
    {
        topDownCarController = GetComponent<TopDownCarController>();
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
        inputVector.y = 1.0f;

        topDownCarController.SetInputVector(inputVector);
    }


    void FollowPlayer()
    {
        if (targetTransform == null)
            targetTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if(targetTransform != null)
            targetPosition = targetTransform.position;
    }

    void FollowWaypoints()
    {

    }

    float TurnTowardsTarget()
    {
        Vector2 vectorToTarget = targetPosition - transform.position;
        vectorToTarget.Normalize();

        float angleToTarget = Vector2.SignedAngle(transform.up, vectorToTarget);
        angleToTarget *= -1;

        float steerAmount = angleToTarget / 45.0f;

        steerAmount = Mathf.Clamp(steerAmount, -1.0f , 1.0f);

        return steerAmount;
    }
}
