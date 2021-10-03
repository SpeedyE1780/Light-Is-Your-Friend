using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : GunController
{
    [Header("Shotgun Stats")]
    [Range(3 , 10)]
    public int NumberOfBullets;
    [Range(0,45)]
    public int SpreadAngle;

    //Set the weapon type and SFX
    protected override void Start()
    {
        base.Start();
        weaponType = WeaponType.Shotgun;
        weaponSFX = SoundManager.Instance.Shotgun;
    }

    protected override IEnumerator Shoot()
    {
        //Alert surrounding zombies
        AlertZombies();

        for (int i = 0; i < NumberOfBullets; i++)
        {
            //Shoot bullet with a random direction
            ShootBullet(Quaternion.Euler(Random.Range(-SpreadAngle, SpreadAngle), Random.Range(-SpreadAngle, SpreadAngle), 0) * WeaponTip.forward);
        }

        yield return null;
    }

#if UNITY_EDITOR

    //Draw cone indicating direction of bullets
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.DrawLine(WeaponTip.position, WeaponTip.position + Quaternion.Euler(SpreadAngle, 0, 0) * WeaponTip.forward * WeaponRange);
        Gizmos.DrawLine(WeaponTip.position, WeaponTip.position + Quaternion.Euler(-SpreadAngle, 0, 0) * WeaponTip.forward * WeaponRange);
        Gizmos.DrawLine(WeaponTip.position, WeaponTip.position + Quaternion.Euler(0, SpreadAngle, 0) * WeaponTip.forward * WeaponRange);
        Gizmos.DrawLine(WeaponTip.position, WeaponTip.position + Quaternion.Euler(0, -SpreadAngle, 0) * WeaponTip.forward * WeaponRange);
    }

#endif
}