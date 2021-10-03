using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deodorant : MonoBehaviour , IInteractable
{
    public float Duration;
    readonly string interactionText = "Press E to pick up Deodorant";

    public void Interact(PlayerController player)
    {
        player.HideScent(Duration); //Hide the player scent
        EventManager.setDeodorant(Duration); //Show deodorant icon on UI
        PoolManager.Instance.AddToPool(PoolID.PickUp, gameObject); //Add object to pool
    }

    //Get the interaction text to display on the ui
    public string GetInteractionText()
    {
        return interactionText;
    }
}