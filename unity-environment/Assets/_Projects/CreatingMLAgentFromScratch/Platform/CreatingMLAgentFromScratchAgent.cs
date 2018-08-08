using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CreatingMLAgentFromScratchAgent : Agent
{
    [SerializeField] private GameObject _ball = null;
    private Rigidbody _ballRigidbody = null;
    private Vector3 _ballStartPosition = Vector3.zero;

    private void Start ()
    {
        _ballRigidbody = _ball.GetComponent<Rigidbody>();
        Assert.IsNotNull(_ballRigidbody);

        _ballStartPosition = _ball.transform.position;
    }

    public override void CollectObservations ()
    {
        var state = new List<float>()
        {
            transform.rotation.x,
            transform.rotation.z,
            _ball.transform.position.x - transform.position.x,
            _ball.transform.position.y - transform.position.y,
            _ball.transform.position.z - transform.position.z,
            _ballRigidbody.velocity.x,
            _ballRigidbody.velocity.y,
            _ballRigidbody.velocity.z,
        };
        AddVectorObs(state);
    }

    public override void AgentReset ()
    {
        transform.rotation = Quaternion.identity;
        _ballRigidbody.velocity = Vector3.zero;
        _ballRigidbody.MovePosition(_ballStartPosition);
    }

    /// <summary>
    /// Controls actual movement of the platform.
    /// Converts Q-values to actions.
    /// </summary>
    /// <param name="vectorAction"></param>
    /// <param name="textAction"></param>
    public override void AgentAction (float[] vectorAction, string textAction)
    {
        Assert.IsNotNull(brain);

        // If using continuouse actions.
        if (brain.brainParameters.vectorActionSpaceType == SpaceType.continuous)
        {
            // Use vectorAactions as continuous values.
            transform.Rotate(Vector3.forward, vectorAction[0]);
            transform.Rotate(Vector3.right, vectorAction[1]);

            // Reward if ball hasn't been dropped.
            if (!IsDone())
            {
                SetReward(0.1f);
            }
        }
        // If using discrete actions.
        else
        {
            int action = (int)vectorAction[0];
            float tiltSpeed = 2f;

            // Z-rotation.
            if (action == 0 || action == 1)
            {
                if (action == 0)
                {
                    transform.Rotate(Vector3.back, tiltSpeed);
                }
                else
                {
                    transform.Rotate(Vector3.forward, tiltSpeed);
                }
            }

            // X-rotation.
            if (action == 2 || action == 3)
            {
                if (action == 2)
                {
                    transform.Rotate(Vector3.left, tiltSpeed);
                }
                else
                {
                    transform.Rotate(Vector3.right, tiltSpeed);
                }
            }

            // Reward if ball hasn't been dropped.
            if (!IsDone())
            {
                SetReward(0.1f);
            }
        }
        var positionDelta = _ball.transform.position - transform.position;

        // Reward if ball has been dropped.
        if (positionDelta.x > 0.6f || positionDelta.y < -1f || positionDelta.z > 0.6f)
        {
            Done();
            SetReward(-1f);
        }
    }
}
