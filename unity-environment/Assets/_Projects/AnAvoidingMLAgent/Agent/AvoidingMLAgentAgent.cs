using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class AvoidingMLAgentAgent : Agent
{
    [SerializeField] private GameObject _topBorder;
    [SerializeField] private GameObject _bottomBorder;
    [SerializeField] private GameObject _leftBorder;
    [SerializeField] private GameObject _rightBorder;
}
