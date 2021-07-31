using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Batteries : MonoBehaviour, IInteractable
{
    [Range(0 , 10)]
    public float ChargeAmount;

    readonly string interactionText = "Press E to pick up Batteries";

    public void Interact(PlayerController player)
    {
        player.ChargeFlashlight(ChargeAmount); //Charge flashlight
        PoolManager.Instance.AddToPool(PoolID.PickUp, gameObject); //Add object to pool
    }

    //Get the interaction text to display on the ui
    public string GetInteractionText()
    {
        return interactionText;
    }
}