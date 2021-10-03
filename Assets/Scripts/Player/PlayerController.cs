using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public FlashLightController FlashLight;
    public Transform PickUpHand;

    //Disables the zombies from detecting the player without seeing the player
    readonly private int PlayerLayer = 8; //Layer where zombies can detect player
    readonly private int CoveredLayer = 0; //Layer where zombies can't detect player

    //Animator parameters
    readonly private string walkingBool = "isRunning";
    readonly private string pickUpTrigger = "PickUp";
    readonly private string hasGunBool = "hasGun";
    readonly private string hasMeleeBool = "hasMelee";
    readonly private string meleeTrigger = "MeleeHit";

    private GameObject currentInteractable;
    private bool weaponPickUp;
    bool doorInteract;
    bool isPickingUp;
    private PlayerMovement playerMovement;
    private Animator playerAnimator;
    private PlayerShooting playerShooting;
    public PlayerShooting GetPlayerShooting => playerShooting;
    
    private void Awake()
    {
        playerShooting = GetComponent<PlayerShooting>();
        playerShooting.Controller = this;
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimator = GetComponent<Animator>();
        currentInteractable = null;
        isPickingUp = false;

        EventManager.toggleCharacters += TogglePlayer;
    }

    IEnumerator UpdateMovementAnimation()
    {
        while(true)
        {
            //Switch between walking and idle animations
            yield return new WaitUntil(() => playerAnimator.GetBool(walkingBool) != playerMovement.IsMoving);
            
            //Set the walking bool and update the sound
            playerAnimator.SetBool(walkingBool, playerMovement.IsMoving);

            if (playerMovement.IsMoving)
                SoundManager.Instance.FootSteps.Play();
            else
                SoundManager.Instance.FootSteps.Stop();
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Let the zombie chase the player if you collide with it
        if (collision.gameObject.CompareTag("Zombie"))
        {
            Zombie zombie = collision.gameObject.GetComponent<Zombie>();
            if(zombie != null)
            {
                zombie.ChasePlayer(transform);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Make sure player isn't already picking up and object
        if(!isPickingUp)
        {
            //Pick up an interactable
            if (other.CompareTag("PickUp"))
            {
                currentInteractable = other.gameObject;
                StartCoroutine(nameof(StartInteraction));

                doorInteract = false;
                weaponPickUp = false;
            }

            //Pick up a weapon
            else if (other.CompareTag("Weapon"))
            {
                currentInteractable = other.gameObject;
                StartCoroutine(nameof(StartInteraction));

                doorInteract = false;
                weaponPickUp = true;
            }

            //Open/Close door
            else if(other.CompareTag("Door"))
            {
                currentInteractable = other.gameObject;
                StartCoroutine(nameof(StartInteraction));

                weaponPickUp = false;
                doorInteract = true;
            }
        }

        //Player reached the end
        if(other.CompareTag("EndGame"))
        {
            EventManager.toggleCharacters.Invoke(0);
            EventManager.endGame.Invoke();
            playerAnimator.SetBool(walkingBool, false);
        }

        //Show the first zombie dialogue
        else if(other.CompareTag("FirstZombie"))
        {
            other.gameObject.SetActive(false);
            EventManager.toggleCharacters.Invoke(0);
            EventManager.firstZombie.Invoke();
            playerAnimator.SetBool(walkingBool, false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Hide interaction text
        if(other.gameObject == currentInteractable)
        {
            StopCoroutine(nameof(StartInteraction));
            UIManager.Instance.HideInteractionText();
        }
    }

    IEnumerator StartInteraction()
    {
        //Show interaction text and wait for player to press e to start interaction
        UIManager.Instance.SetInteractionText(currentInteractable.GetComponent<IInteractable>().GetInteractionText());
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

        //If player isn't interacting with a door start pickup animation else directly start interaction
        if(!doorInteract)
        {
            playerAnimator.SetTrigger(pickUpTrigger);
            isPickingUp = true;
        }
        else
        {
            currentInteractable.GetComponent<IInteractable>().Interact(this);
        }
    }

    //Called by pick up animation
    void PickUpObject()
    {
        //Position the pickup in player's hand
        currentInteractable.transform.SetParent(PickUpHand);
        currentInteractable.transform.localPosition = Vector3.zero;

        if (weaponPickUp)
            if (!playerAnimator.GetBool(hasGunBool) && !playerAnimator.GetBool(hasMeleeBool)) //If no weapon animation is playing
                SetWeaponAnimation(currentInteractable.GetComponent<WeaponController>().GetWeaponType); //Set the animation of the player based on the weapon type
    }

    //Called in the beginning and end of pick up animation
    void PickingUpInteractbale(int toggle)
    {
        //Hide interaction text
        UIManager.Instance.HideInteractionText();

        //Stop player movement
        ToggleMovement(toggle);
        
        //Turn off weapon while picking up object
        playerShooting.ToggleWeapon(toggle);

        //True at the end of the animation
        if (toggle != 0)
        {
            //Interact with object
            currentInteractable.GetComponent<IInteractable>().Interact(this);
            isPickingUp = false;
        }
    }

    //Set the animation based on the current weapon
    public void SetWeaponAnimation(WeaponType weaponType)
    {
        bool gunAnimation = weaponType != WeaponType.Melee;
        playerAnimator.SetBool(hasGunBool, gunAnimation);
        playerAnimator.SetBool(hasMeleeBool, !gunAnimation);
    }

    //Start melee animation
    public void MeleeHit()
    {
        playerAnimator.SetTrigger(meleeTrigger);
    }

    void ToggleMovement(int toggle)
    {
        this.enabled = toggle != 0;
        playerMovement.enabled = toggle != 0;
        playerShooting.enabled = toggle != 0;

        if (toggle != 0)
        {
            StartCoroutine(nameof(UpdateMovementAnimation));
        }
        else
        {
            StopCoroutine(nameof(UpdateMovementAnimation));
            SoundManager.Instance.FootSteps.Stop();
        }
    }

    public void TogglePlayer(int toggle)
    {
        ToggleMovement(toggle);
        FlashLight.enabled = toggle != 0;
    }

    //Change player layer avoids being added in the zombie overlap shere
    public void HideScent(float duration)
    {
        gameObject.layer = CoveredLayer;

        //Cancel Invoke and Invoke again
        CancelInvoke(nameof(SmellRecovered));
        Invoke(nameof(SmellRecovered), duration);
    }

    //Set player layer to default layer
    void SmellRecovered()
    {
        gameObject.layer = PlayerLayer;
    }

    //Charge flashlight
    public void ChargeFlashlight(float amount)
    {
        FlashLight.ChargeFlashLight(amount);
    }
}