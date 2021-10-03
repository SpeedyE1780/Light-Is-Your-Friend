using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigateGunShot : State
{
    public InvestigateGunShot(Zombie zombie, Vector3 GunShotOrigin)
                : base(zombie)
    {
        stateName = StateName.InvestigateGunShot; // Set name of current state.
        _zombie.GetAgent.SetDestination(GunShotOrigin); //Set the position of the gun shot as destination
    }

    protected override void Update()
    {
        //If zombie can detect the player chase him
        if (_zombie.CanDetectPlayer())
        {
            nextState = new Chase(_zombie);
            stage = StateStage.Exit;
        }

        //When zombie reachs the destination go to idle state
        if(_zombie.GetAgent.remainingDistance < 1f)
        {
            nextState = new Idle(_zombie);
            stage = StateStage.Exit;
        }
    }
}