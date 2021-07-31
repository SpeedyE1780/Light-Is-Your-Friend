using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticShotgun : Shotgun
{
    [Tooltip("Number of Shoots / Second")]
    public int RateOfFire;

    //Set the weapon type and SFX
    protected override void Start()
    {
        base.Start();
        weaponType = WeaponType.AutomaticShotgun;
    }

    //Keep shooting while mouse button down
    protected override IEnumerator Shoot()
    {
        WaitForSeconds cooldown = new WaitForSeconds(1 / RateOfFire);

        //Call shotgun's shoot method and wait for the cooldown
        while(true)
        {
            //Call the shotgun shoot
            StartCoroutine(base.Shoot());

            //Reset the critical shot
            criticalShot = Random.Range(0, 101) <= CriticalChance;

            yield return cooldown;
        }
    }

#if UNITY_EDITOR

    //Make sure values aren't less than 0
    protected override void OnValidate()
    {
        base.OnValidate();
        RateOfFire = RateOfFire <= 0 ? 0 : RateOfFire;
    }

#endif
}