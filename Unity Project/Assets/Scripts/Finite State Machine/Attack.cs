using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : State
{
    public Attack(Zombie zombie)
                : base(zombie)
    {
        stateName = StateName.Attack; // Set name to correct state.
    }

    protected override void Update()
    {
        if (!_zombie.CanAttack())
        {
            nextState = new Chase(_zombie); // If zombie can't attack player, set chase as nextState.
            stage = StateStage.Exit; // Set stage correctly as we are finished with Attack state.
        }
    }
}