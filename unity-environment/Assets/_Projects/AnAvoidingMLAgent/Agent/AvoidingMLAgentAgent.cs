using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class AvoidingMLAgentAgent : Agent
{
    [SerializeField] private GameObject _topBorder = null;
    [SerializeField] private GameObject _bottomBorder = null;
    [SerializeField] private GameObject _leftBorder = null;
    [SerializeField] private GameObject _rightBorder = null;
    private float _force = 10f;
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
        var state = new List<float>()
        {
            Vector2.Distance(transform.position, _topBorder.transform.position),
            Vector2.Distance(transform.position, _bottomBorder.transform.position),
            Vector2.Distance(transform.position, _leftBorder.transform.position),
            Vector2.Distance(transform.position, _rightBorder.transform.position),
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
}
