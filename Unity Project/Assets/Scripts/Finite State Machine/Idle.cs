using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    readonly float endTime;
    public Idle(Zombie zombie)
                : base(zombie)
    {
        stateName = StateName.Idle; // Set name of current state.
        endTime = Time.time + Random.Range(1.0f, 5.0f); // Set the idle end time to a random value between 1 and 5
    }

    protected override void Update()
    {
        //If the zombie can detect player set state to chase him
        if (_zombie.CanDetectPlayer())
        {
            nextState = new Chase(_zombie);
            stage = StateStage.Exit;
        }

        //Patrol after a certain amount of time has passed
        else if (endTime <= Time.time)
        {
            nextState = new Patrol(_zombie);
            stage = StateStage.Exit;
        }
    }
}