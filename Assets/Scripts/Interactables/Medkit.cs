using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour, IInteractable
{
    [Range(0, 25)]
    public int HealAmount;

    readonly string interactionText = "Press E to pick up Medkit";

    //Heal player and add gameobject to pool
    public void Interact(PlayerController player)
    {
        player.GetComponent<PlayerHealth>().Heal(HealAmount);
        PoolManager.Instance.AddToPool(PoolID.PickUp, gameObject);
    }

    //Get the interaction text to display on the ui
    public string GetInteractionText()
    {
        return interactionText;
    }
}