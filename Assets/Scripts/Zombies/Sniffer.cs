using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sniffer : Zombie
{
    public override bool CanDetectPlayer()
    {
        //Get all players in detection raduis
        Collider[] detectedPlayers = Physics.OverlapSphere(transform.position, DetectionRaduis, PlayerLayer);

        if (detectedPlayers.Length > 0)
        {
            //Set the target to the first detected player
            targetPlayer = detectedPlayers[0].transform;

            //Get the closest player and set it to the target
            foreach(Collider player in detectedPlayers)
            {
                if(Vector3.Distance(transform.position , player.transform.position) < Vector3.Distance(transform.position , targetPlayer.transform.position))
                {
                    targetPlayer = player.transform;
                }
            }

            return true;
        }

        else
        {
            return false;
        }
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        //Draw Attack Sphere
        base.OnDrawGizmos();

        //Draw Detection Sphere
        if (Application.isPlaying && currentState != null)
        {
            if (currentState.stateName == StateName.Chase || currentState.stateName == StateName.Attack)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
        }

        Gizmos.DrawWireSphere(transform.position, DetectionRaduis);
    }
#endif
}