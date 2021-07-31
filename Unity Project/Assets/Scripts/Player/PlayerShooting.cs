using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public LayerMask ZombieLayer;
    public Transform WeaponParent;
    List<WeaponController> playerWeapons;
    WeaponController currentWeapon;
    List<WeaponType> weaponTypes;
    
    public PlayerController Controller { get; set; }

    private void Start()
    {
        playerWeapons = new List<WeaponController>();
        weaponTypes = new List<WeaponType>();
        currentWeapon = null;
    }

    // Update is called once per frame
    void Update()
    {
        //Check that player is switching weapons
        if (playerWeapons.Count > 1 && Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            //Turn off current weapon
            currentWeapon.DeactivateWeapon();
            int index;

            //Set current weapon to previous weapon
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                index = playerWeapons.IndexOf(currentWeapon) - 1 >= 0 ? playerWeapons.IndexOf(currentWeapon) - 1 : playerWeapons.Count - 1;
            }

            //Set current weapon to next weapon
            else
            {
                index = playerWeapons.IndexOf(currentWeapon) + 1 < playerWeapons.Count ? playerWeapons.IndexOf(currentWeapon) + 1 : 0;
            }

            //Update current weapon set it active and update game ui
            currentWeapon = playerWeapons[index];
            currentWeapon.gameObject.SetActive(true);
            UIManager.Instance.SetWeaponImage(currentWeapon.WeaponIcon);

            //Update player animation
            Controller.SetWeaponAnimation(currentWeapon.GetWeaponType);
        }
        
        //If current weapon is not null shoot
        if(currentWeapon != null)
        {
            //Start shooting with gun
            if(currentWeapon.GetWeaponType != WeaponType.Melee)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    currentWeapon.StartShoot();
                }

                if (Input.GetMouseButtonUp(0))
                {
                    currentWeapon.StopShoot();
                }
            }

            //Play melee animation
            else
            {
                if(Input.GetMouseButtonDown(0))
                {
                    Controller.MeleeHit();
                }
            }
            
        }
    }

    public void AddWeapon(WeaponController weapon)
    {
        //If player don't have weapon delete it
        if(!weaponTypes.Contains(weapon.GetWeaponType))
        {
            playerWeapons.Add(weapon); //Add weapon to list
            weaponTypes.Add(weapon.GetWeaponType); //Add weapon type to list
            weapon.SetZombieLayer(ZombieLayer);

            //Set the weapon position in the correct hand
            weapon.transform.SetParent(weapon.GetWeaponType != WeaponType.Melee ? WeaponParent : Controller.PickUpHand);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.rotation = Quaternion.LookRotation(WeaponParent.forward , Vector3.up);
            
            //Update current weapon and show it's ui image
            if (currentWeapon == null)
            {
                currentWeapon = weapon;
                UIManager.Instance.SetWeaponImage(currentWeapon.WeaponIcon);
            }

            //Deactivate weapon
            else
            {
                weapon.DeactivateWeapon();
            }
        }

        else
        {
            PoolManager.Instance.AddToPool(PoolID.WeaponPickUp, weapon.gameObject);
        }
    }

    //Turn weapon on/off
    public void ToggleWeapon(int toggle)
    {
        if (currentWeapon != null)
            currentWeapon.gameObject.SetActive(toggle != 0);
    }

    //Called from animation
    public void ApplyMeleeDamage()
    {
        currentWeapon.StartShoot();
    }
}