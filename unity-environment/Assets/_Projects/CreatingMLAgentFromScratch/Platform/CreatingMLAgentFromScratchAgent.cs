using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CreatingMLAgentFromScratchAgent : Agent
{
    [SerializeField] private GameObject _ball = null;

    public override void CollectObservations ()
    {
        var ballRigidbody = _ball.GetComponent<Rigidbody>();
        Assert.IsNotNull(ballRigidbody);

        var state = new List<float>()
        {
            transform.rotation.x,
            transform.rotation.z,
            _ball.transform.position.x - transform.position.x,
            _ball.transform.position.y - transform.position.y,
            _ball.transform.position.z - transform.position.z,
            ballRigidbody.velocity.x,
            ballRigidbody.velocity.y,
            ballRigidbody.velocity.z,
        };

        AddVectorObs(state);
    }
}
