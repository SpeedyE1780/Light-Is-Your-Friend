using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : Zombie
{
    public float DetectionAngle;

    public override bool CanDetectPlayer()
    {
        //Get all players in detection raduis
        Collider[] detectedPlayers = Physics.OverlapSphere(transform.position, DetectionRaduis, PlayerLayer);

        if (detectedPlayers.Length > 0)
        {
            //Set the target to null
            targetPlayer = null;

            //Get the closest player and set it to the target
            foreach (Collider player in detectedPlayers)
            {
                Vector3 direction = player.transform.position - transform.position;

                //Check that player is in front of zombie and no walls is blocking the view
                bool canSee = Vector3.Angle(transform.forward, direction) <= DetectionAngle;
                Physics.Raycast(transform.position, direction, out RaycastHit hit, DetectionRaduis, PlayerLayer);
                canSee = canSee && hit.transform == player.transform;

                //If target is null return true else check if currentplayer is closer than the target
                bool closerTarget = targetPlayer == null || Vector3.Distance(transform.position, player.transform.position) < Vector3.Distance(transform.position, targetPlayer.transform.position);
                
                if (canSee && closerTarget)
                {
                    targetPlayer = player.transform;
                }
            }

            //If zombie found a target return true
            return targetPlayer != null;
        }

        else
        {
            return false;
        }
    }

#if UNITY_EDITOR

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.white;

        //Draw Left & Right Bounds
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, DetectionAngle, 0) * transform.forward * DetectionRaduis);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, -DetectionAngle, 0) * transform.forward * DetectionRaduis);
    }

#endif
}