using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase : State
{
    readonly float chaseTime;
    public Chase(Zombie zombie, bool playerDetected = false)
                : base(zombie)
    {
        stateName = StateName.Chase; // State set to match what zombie is doing.

        //Make sure zombie follows player for 3 seconds if player collided with the zombie
        chaseTime = playerDetected ? Time.time + 3 : 0;

        SoundManager.Instance.GetChaseSound.Play(); //Play sound to alert player that zombie is chasing him
    }

    protected override void Update()
    {
        _zombie.GetAgent.SetDestination(_zombie.GetTarget.position);  // Set the player position as destination
        if (_zombie.GetAgent.hasPath)                       // ...check if agent has a path yet.
        {
            //If zombie can't detect the player go back to patrol state
            if (chaseTime <= Time.time && !_zombie.CanDetectPlayer())
            {
                nextState = new Patrol(_zombie);
                stage = StateStage.Exit;
            }
            
            //If zombie can attack the player go to attack state
            else if(_zombie.CanAttack())
            {
                nextState = new Attack(_zombie);
                stage = StateStage.Exit;
            }
        }
    }
}