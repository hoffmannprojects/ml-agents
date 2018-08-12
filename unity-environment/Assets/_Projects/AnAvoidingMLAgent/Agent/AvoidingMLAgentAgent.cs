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


}
