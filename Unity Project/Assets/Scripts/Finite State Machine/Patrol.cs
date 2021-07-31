using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : State
{
    int currentIndex = -1;
    readonly List<Transform> waypoints;
    public Patrol(Zombie zombie)
                : base(zombie)
    {
        stateName = StateName.Patrol; // Set name of current state.
        waypoints = _zombie.GetPatrolPoints; //Get the zombie patrol points
    }

    protected override void Enter()
    {
        float lastDist = Mathf.Infinity; // Store distance between zombie and nearest waypoint.

        // Find the closest waypoint
        foreach(Transform waypoint in waypoints)
        {
            float distance = Vector3.Distance(_zombie.transform.position, waypoint.position);
            if (distance < lastDist)
            {
                currentIndex = waypoints.IndexOf(waypoint);
                lastDist = distance;
            }
        }

        //Set the waypoint as the destination
        _zombie.GetAgent.SetDestination(waypoints[currentIndex].position);

        base.Enter();
    }

    protected override void Update()
    {
        //Check if zombie reached the waypoint
        if (_zombie.GetAgent.remainingDistance < 1)
        {
            // If agent has reached end of waypoint list, go back to the first one, otherwise move to the next one.
            currentIndex = (currentIndex + 1) % waypoints.Count;

            _zombie.GetAgent.SetDestination(waypoints[currentIndex].position); // Set agents destination to position of next waypoint.
        }

        //If zombie can detect player chase him
        if(_zombie.CanDetectPlayer())
        {
            nextState = new Chase(_zombie);
            stage = StateStage.Exit;
        }
    }
}