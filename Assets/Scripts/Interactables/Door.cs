using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    readonly string animatorBool = "DoorOpened";
    Animator doorAnimator;

    private void Awake()
    {
        doorAnimator = GetComponent<Animator>();
        doorAnimator.SetBool(animatorBool, false); //Make sure door starts closed
    }
    public string GetInteractionText()
    {
        //Checks wether we will open or close the door
        string doorState = doorAnimator.GetBool(animatorBool) ? "close" : "open";
        return $"Press E to {doorState} the Door";
    }

    public void Interact(PlayerController player)
    {
        //Start open/close door animation
        doorAnimator.SetBool(animatorBool, !doorAnimator.GetBool(animatorBool));
    }
}