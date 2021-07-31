using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : WeaponController
{
    public float AlertRange; 
    public Transform WeaponTip;

    private LineRenderer lineRenderer;

    protected virtual void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    //Show the range of the weapon
    private void Update()
    {
        //Set the first position of the line renderer at the tip of the weapon
        lineRenderer.SetPosition(0, WeaponTip.position);
        
        //Set the last position on the object the raycast hit else at max range
        if (Physics.Raycast(WeaponTip.position, WeaponTip.forward, out RaycastHit hit, WeaponRange))
        {
            if (hit.transform != null)
                lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.SetPosition(1, WeaponTip.position + WeaponTip.forward * WeaponRange);
        }
    }

    //Turn on the lineRenderer once the weapon is picked up
    public override void Interact(PlayerController player)
    {
        base.Interact(player);
        lineRenderer.enabled = true;
    }

    //Turn off line renderer while shooting
    public override void StartShoot()
    {
        //Prevent shooting while sound is playing
        if(!weaponSFX.isPlaying)
        {
            base.StartShoot();
            lineRenderer.enabled = false;
            weaponSFX.Play();
        }
    }

    //Shoot a raycast from the weapon tip to given position
    protected void ShootBullet(Vector3 direction)
    {
        Physics.Raycast(WeaponTip.position, direction, out RaycastHit hit, WeaponRange);

        //Make sure raycast hit a zombie
        if (hit.transform != null && hit.transform.CompareTag("Zombie"))
        {
            hit.transform.GetComponent<Health>().TakeDamage(criticalShot ? Mathf.RoundToInt(WeaponDamage * CriticalMulitiplier) : WeaponDamage);
            Debug.DrawRay(WeaponTip.position, hit.point - WeaponTip.position, criticalShot ? Color.yellow : Color.green, 1);
        }

        else
        {
            Debug.DrawRay(WeaponTip.position, direction * WeaponRange, Color.red, 1);
        }

        //Get a bullet to shoot
        BulletController bullet = PoolManager.Instance.GetPooledObject(PoolID.GunBullets).GetComponent<BulletController>();
        bullet.SetBullet(WeaponTip.position ,direction);

        criticalShot = false;
    }

    //Alert surrounding zombies that a gunshot was fired
    protected void AlertZombies()
    {
        Collider[] zombies = Physics.OverlapSphere(transform.position, AlertRange, zombieLayer);

        if (zombies.Length > 0)
        {
            foreach (Collider zombie in zombies)
            {
                zombie.GetComponent<Zombie>().InvestigateGunShot(transform.position);
            }
        }
    }

    //Turn on line renderer after shooting
    public override void StopShoot()
    {
        base.StopShoot();
        lineRenderer.enabled = true;
    }

#if UNITY_EDITOR

    //Draw bullet trajectory
    protected virtual void OnDrawGizmosSelected()
    {
        if (WeaponTip == null)
            return;

        Gizmos.DrawWireSphere(transform.position, AlertRange);
        Gizmos.DrawLine(WeaponTip.position, WeaponTip.position + WeaponTip.forward * WeaponRange);
    }

    //Make sure values aren't negative
    protected override void OnValidate()
    {
        base.OnValidate();
        AlertRange = AlertRange <= 0 ? 0 : AlertRange;
    }

#endif

}