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
}
