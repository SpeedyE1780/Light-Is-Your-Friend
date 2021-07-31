using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : GunController
{
    [Header("Rifle Stats")]
    [Tooltip("Number of Shoots / Second")]
    public int RateOfFire;

    //Set the weapon type and SFX
    protected override void Start()
    {
        base.Start();
        weaponType = WeaponType.AutomaticRifle;
        weaponSFX = SoundManager.Instance.Assault;
    }

    protected override IEnumerator Shoot()
    {
        WaitForSeconds cooldown = new WaitForSeconds(1 / RateOfFire);
        //SoundManager.Instance.Assault.Play();
        while(true)
        {
            //Alert surroungind zombies
            AlertZombies();

            //Shoot bullet in the weapon's forward direction
            ShootBullet(WeaponTip.forward);

            //Reset the critical shot
            criticalShot = Random.Range(0, 101) <= CriticalChance;

            yield return cooldown;
        }
    }

    public override void StopShoot()
    {
        base.StopShoot();
        weaponSFX.Stop();
    }

#if UNITY_EDITOR

    //Make sure values are not less than 0
    protected override void OnValidate()
    {
        base.OnValidate();
        RateOfFire = RateOfFire <= 0 ? 0 : RateOfFire;
    }

#endif
}