using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class AvoidingMLAgentAgent : Agent
{
    private float _force = 0.5f;
    private Vector2 _startPosition = Vector2.zero;
    private Rigidbody2D _rigidBody2D = null;
    private bool _hasCrashed = false;

    /// <summary>
    /// Only initializes when called from the ml-agents system,
	/// not every time the game runs.
    /// </summary>
    public override void InitializeAgent()
    {
        _startPosition = transform.position;

        _rigidBody2D = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(_rigidBody2D);
    }

    public override void CollectObservations()
    {
        // Raycasts up.
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, Vector2.up);
        var distanceUp = hit2D.distance;
       
        // Raycasts down.
        hit2D = Physics2D.Raycast(transform.position, Vector2.down);
        var distanceDown = hit2D.distance;
        
        // Raycasts left.
        hit2D = Physics2D.Raycast(transform.position, Vector2.left);
        var distanceLeft = hit2D.distance;

        // Raycasts right.
        hit2D = Physics2D.Raycast(transform.position, Vector2.right);
        var distanceRight = hit2D.distance;

        var state = new List<float>()
        {
            distanceUp,
            distanceDown,
            distanceLeft,
            distanceRight,
            _rigidBody2D.velocity.x,
            _rigidBody2D.velocity.y,
        };
        AddVectorObs(state);
    }

    /// <summary>
    /// Reports collisions of collider & Rigidbody on the same GameObject.
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        _hasCrashed = true;
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        int action = (int)vectorAction[0];

        // Assert.IsNotNull(brain);

        /// Discrete is in this case only included for practice.
        /// Only use discrete for discrete choices (eg. which weapon?, which stage?).
        /// Here, the amount matters, which makes continuous a better fit.
        if (brain.brainParameters.vectorActionSpaceType == SpaceType.discrete)
        {
            switch (action)
            {
                case 0:
                    _rigidBody2D.AddForce(Vector2.up * _force);
                    break;
                case 1:
                    _rigidBody2D.AddForce(Vector2.down * _force);
                    break;
                case 2:
                    _rigidBody2D.AddForce(Vector2.left * _force);
                    break;
                case 3:
                    _rigidBody2D.AddForce(Vector2.right * _force);
                    break;
                default:
                    _rigidBody2D.AddForce(Vector2.zero);
                    break;
            }
            if (!IsDone())
            {
                SetReward(0.1f);
            }
        }
        /// <summary>
        /// Continuous action type.
		/// Allows for applying different amounts.
        /// </summary>
        /// <value></value>
        else
        {
            _rigidBody2D.AddForce(Vector2.up * _force * vectorAction[0]);
            _rigidBody2D.AddForce(Vector2.down * _force * vectorAction[1]);
            _rigidBody2D.AddForce(Vector2.left * _force * vectorAction[2]);
            _rigidBody2D.AddForce(Vector2.right * _force * vectorAction[3]);

            if (!IsDone())
            {
                SetReward(0.1f);
            }
        }
        if (_hasCrashed)
        {
            Done();
            SetReward(-1.0f);
        }
    }

    /// <summary>
    /// Called when IsDone() reports true or
    /// when max steps is reached.
    /// </summary>
    public override void AgentReset()
    {
        transform.position = _startPosition;
        _rigidBody2D.velocity = Vector2.zero;
        _hasCrashed = false;
    }
}
