using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst : GunController
{
    //Set the weapon type and SFX
    protected override void Start()
    {
        base.Start();
        weaponType = WeaponType.BurstRifle;
        weaponSFX = SoundManager.Instance.Burst;
    }

    //Shoot 3 bullets
    protected override IEnumerator Shoot()
    {
        WaitForSeconds cooldown = new WaitForSeconds(0.05f);

        for (int i = 0; i < 3; i++)
        {
            //Alert surrounding zombies on each shoot
            AlertZombies();

            //Shoot bullet using the weapon's forward direction
            ShootBullet(WeaponTip.forward);

            //SoundManager.Instance.Burst.Play();

            yield return cooldown;
        }
    }
}