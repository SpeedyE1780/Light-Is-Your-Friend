using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    // 'Stage' - where we are in the running of a STATE.
    public enum StateStage
    {
        Enter, Update, Exit
    };

    public StateName stateName; // To store the name of the state.
    protected StateStage stage; // To store the stage the state is in.
    protected Zombie _zombie; // To store the zombie gameobject
    protected State nextState; // Next state after exiting the current state

    public State(Zombie zombie)
    {
        _zombie = zombie;
        stage = StateStage.Enter;
    }

    // Phases as you go through the state.
    protected virtual void Enter() 
    { 

        stage = StateStage.Update;
        _zombie.SetAgent(stateName); //Set all the zombie parameters based on the state name

    }

    protected virtual void Update() 
    { 
        stage = StateStage.Update; 
    }

    protected virtual void Exit()
    { 

        stage = StateStage.Exit;
        _zombie.ResetTrigger(stateName); //Reset the trigger of the current state in case it's still active

    }

    // The method that will get run from outside and progress the state through each of the different stages.
    public State Process()
    {
        if (stage == StateStage.Enter)
            Enter();

        if (stage == StateStage.Update)
            Update();

        //Exit from current state and return next state
        if (stage == StateStage.Exit)
        {
            Exit();
            return nextState;
        }

        return this;
    }

    //Investigate a position
    public void Investigate(Vector3 position)
    {
        //Make sure that zombie isn't already chasing or attacking the player
        if(stateName != StateName.Chase || stateName != StateName.Attack)
        {
            nextState = new InvestigateGunShot(_zombie, position);
            stage = StateStage.Exit;
        }
    }

    //Chase the player if he collides with zombie
    public void Chase()
    {
        //Make sure that zombie isn't already chasing or attacking the player
        if (stateName != StateName.Chase || stateName != StateName.Attack)
        {
            nextState = new Chase(_zombie , true);
            stage = StateStage.Exit;
        }
    }
}

// 'States' that the zombie could be in.
public enum StateName
{
    Idle, Patrol, Chase, Attack, InvestigateGunShot
}