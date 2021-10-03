using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : WeaponController
{
    //Set weapon type and SFX
    void Start()
    {
        weaponType = WeaponType.Melee;
        weaponSFX = SoundManager.Instance.MeleeSwing;
    }

    protected override IEnumerator Shoot()
    {
        GetZombies();
        weaponSFX.Play();
        yield return null;
    }

    //Damage all zombies in melee range
    void GetZombies()
    {
        Collider[] zombies = Physics.OverlapSphere(transform.position + transform.parent.forward, WeaponRange, zombieLayer);

        foreach (Collider zombie in zombies)
            zombie.GetComponent<Health>().TakeDamage(WeaponDamage);
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.parent != null ? transform.position + transform.parent.forward : transform.position + Vector3.forward, WeaponRange);
    }

#endif

}