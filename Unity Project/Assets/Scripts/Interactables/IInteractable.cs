using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    //Get the interaction text to display on the ui
    string GetInteractionText();

    void Interact(PlayerController player);
}