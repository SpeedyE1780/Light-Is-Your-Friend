using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MovementSpeed = 3;
    public float RotationSpeed = 10;
    public bool IsMoving => rigidBody.velocity != Vector3.zero;

    Rigidbody rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
    }

    void Update()
    {
        rigidBody.velocity = Input.GetAxis("Vertical") * transform.forward * MovementSpeed + Input.GetAxis("Horizontal") * transform.right * MovementSpeed;

        //Rotate player to look at mouse cursor
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform != null)
            {
                Vector3 lookDirection = hit.point - transform.position;
                lookDirection.y = 0;

                transform.rotation = Quaternion.LookRotation(lookDirection);
            }
        }
    }
}