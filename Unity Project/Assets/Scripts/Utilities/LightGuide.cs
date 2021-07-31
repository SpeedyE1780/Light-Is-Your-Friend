using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGuide : MonoBehaviour
{
    public Transform WaypointParent;
    public Transform Player;
    public float MovementSpeed;
    public float RotationSpeed;
    public float MaxDistance;
    
    List<Transform> waypoints;
    int currentWaypoint;
    Vector3 direction;

    private void Awake()
    {
        EventManager.toggleCharacters += ToggleMovement;
    }

    void ToggleMovement(int toggle)
    {
        this.enabled = toggle != 0;
    }

    private void Start()
    {
        waypoints = new List<Transform>();

        foreach (Transform child in WaypointParent)
            waypoints.Add(child);

        currentWaypoint = 0;
    }

    private void LateUpdate()
    {
        //Prevent moving when player is too far away
        if (Vector3.Distance(transform.position, Player.position) < MaxDistance)
        {
            //Update direction
            direction = waypoints[currentWaypoint].position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), RotationSpeed * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward * MovementSpeed * Time.deltaTime, 0.4f);

            //Go to next waypoint
            if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 3)
            {
                currentWaypoint++;

                //Disable once reached the end
                if (currentWaypoint >= waypoints.Count)
                    this.enabled = false;
            }
        }
    }
}