using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : GunController
{
    //Set the weapon type and SFX
    protected override void Start()
    {
        base.Start();
        weaponType = WeaponType.Pistol;
        weaponSFX = SoundManager.Instance.Pistol;
    }

    protected override IEnumerator Shoot()
    {
        //Alert surrounding zombies on each shoot
        AlertZombies();

        //Shoot bullet using the weapon's forward direction
        ShootBullet(WeaponTip.forward);

       // SoundManager.Instance.Pistol.Play();

        yield return null;
    }
}