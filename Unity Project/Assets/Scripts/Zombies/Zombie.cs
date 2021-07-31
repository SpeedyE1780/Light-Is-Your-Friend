using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Zombie : MonoBehaviour
{
    [Header("General Stats")]
    public Transform PatrolParent;
    public StateStats Stats;
    public LayerMask PlayerLayer;

    [Header("Attack Stats")]
    public float AttackRange;
    public float AttackAngle;
    public int AttackDamage;

    [Header("Detection Stats")]
    public float DetectionRaduis;
    

    List<Transform> PatrolPoints;
    public List<Transform> GetPatrolPoints => PatrolPoints;

    NavMeshAgent agent;
    public NavMeshAgent GetAgent => agent;

    protected Transform targetPlayer;
    public Transform GetTarget => targetPlayer;
    protected State currentState;
    protected Animator zombieAnimator;
    private Health zombieHealth;

    protected virtual void Start()
    {
        //Get agent and initialize patrol points
        agent = GetComponent<NavMeshAgent>();
        zombieAnimator = GetComponent<Animator>();
        zombieHealth = GetComponent<Health>();
        zombieHealth.SetID(PoolID.Zombie);

        //Get all patrol points in parent
        PatrolPoints = new List<Transform>();
        foreach(Transform point in PatrolParent)
        {
            PatrolPoints.Add(point);
        }

        EventManager.toggleCharacters += ToggleZombie;
        EventManager.endGame += KillZombies;
        this.enabled = false;
    }

    protected virtual void ToggleZombie(int toggle)
    {
        //Set the state to idle
        currentState = new Idle(this);
        this.enabled = toggle != 0;
    }

    void KillZombies()
    {
        StopAllCoroutines();
        PoolManager.Instance.AddToPool(PoolID.Zombie, gameObject);
    }

    private void Update()
    {
        //Update the state
        currentState = currentState.Process();
    }

    public virtual bool CanDetectPlayer() { return true; }

    //Check if zombie can attack
    public virtual bool CanAttack()
    {
        //Check that player is in attack range
        if(Vector3.Distance(transform.position , targetPlayer.position) <= AttackRange)
        {
            // Calculate direction and angle to player.
            Vector3 direction = targetPlayer.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            //Check no wall between player and zombie
            if(Physics.Raycast(transform.position , direction , PlayerLayer))
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 1 * Time.deltaTime);
                //Check that zombie can see player to attack
                return angle <= AttackAngle;
            }
        }

        return false;
    }

    //Called from animation
    public virtual void ApplyDamage()
    {
        if (targetPlayer != null)
        {
            targetPlayer.GetComponent<PlayerHealth>().TakeDamage(AttackDamage);
            SoundManager.Instance.ZombiePunch.Play();

        }
    }

    //Called when zombie is in range from a gunshot
    public void InvestigateGunShot(Vector3 position)
    {
        currentState.Investigate(position);
    }

    //Called on collision with player
    public void ChasePlayer(Transform player)
    {
        targetPlayer = player;
        currentState.Chase();
    }

    //Set navmesh stats
    public void SetAgent(StateName state)
    {
        zombieAnimator.SetTrigger(Stats[state].AnimatorTrigger);
        agent.speed = Stats[state].MovementSpeed;
        agent.angularSpeed = Stats[state].AngularSpeed;
        agent.isStopped = Stats[state].IsStopped;
    }

    //Reset the animator trigger
    public void ResetTrigger(StateName state)
    {
        zombieAnimator.ResetTrigger(Stats[state].AnimatorTrigger);
    }

#if UNITY_EDITOR

    //Draw sphere to determine attack range
    protected virtual void OnDrawGizmos()
    {
        if (Application.isPlaying && currentState != null)
        {
            if (currentState.stateName == StateName.Attack)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.yellow;
            }
        }

        //Attack lines
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, AttackAngle, 0) * transform.forward * AttackRange);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, -AttackAngle, 0) * transform.forward * AttackRange);
    }
#endif

}

[System.Serializable]
public class StateStats : SerializableDictionaryBase<StateName, AgentStats> { }