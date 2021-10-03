using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloater : Walker
{
    [Header("Bloater Attack Stats")]
    public float MeleeRange;
    public int SpitDamage;
    public Transform BloaterMouth;

    WaitUntil waitAttacking;

    bool IsAttacking => currentState.stateName == StateName.Attack; //Checks if bloater is attacking
    bool IsSpitting => Vector3.Distance(transform.position, targetPlayer.transform.position) > MeleeRange; //Checks if zombie is spitting or melee
    int CurrentDamge => IsSpitting ? SpitDamage : AttackDamage; //Return the damage based on the bloater attack

    readonly private string isSpittingBool = "IsSpitting";

    private void OnDisable()
    {
        StopCoroutine(nameof(Attacking));
    }

    protected override void ToggleZombie(int toggle)
    {
        base.ToggleZombie(toggle);

        if(toggle != 0)
        {
            StartCoroutine(nameof(Attacking));
        }
        
        waitAttacking = new WaitUntil(() => IsAttacking);
    }

    IEnumerator Attacking()
    {
        while (true)
        {
            yield return waitAttacking;

            while(IsAttacking)
            {
                //Update bool once isSpitting changes value
                if (zombieAnimator.GetBool(isSpittingBool) != IsSpitting)
                {
                    zombieAnimator.SetBool(isSpittingBool, IsSpitting);
                }

                yield return null;
            }
        }
    }

    //Called from animation
    public override void ApplyDamage()
    {
        if(targetPlayer != null)
        {
            targetPlayer.GetComponent<PlayerHealth>().TakeDamage(CurrentDamge);

            //Play spit sound and shoot bullets
            if (IsSpitting)
            {
                SoundManager.Instance.Spit.Play();
                BulletController bullet = PoolManager.Instance.GetPooledObject(PoolID.ZombieBullets).GetComponent<BulletController>();
                bullet.SetBullet(BloaterMouth.position, BloaterMouth.forward);
            }

            //Play punch sound
            else
                SoundManager.Instance.ZombiePunch.Play();
        }
    }

#if UNITY_EDITOR

    //Draw sphere to determine attack range
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        //Attack lines
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, AttackAngle, 0) * transform.forward * MeleeRange);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, -AttackAngle, 0) * transform.forward * MeleeRange);
    }

#endif
}