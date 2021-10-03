using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WeaponController : MonoBehaviour, IInteractable
{
    [Header("Weapon Display")]
    public string WeaponName;
    public Sprite WeaponIcon;

    [Header("Weapon Stats")]
    public int WeaponDamage;
    public float WeaponRange;
    [Range(0,100)]
    public int CriticalChance;
    public float CriticalMulitiplier;

    protected WeaponType weaponType;
    protected LayerMask zombieLayer;
    protected AudioSource weaponSFX;
    protected bool criticalShot;

    public WeaponType GetWeaponType => weaponType;

    //Add weapon to player list
    public virtual void Interact(PlayerController player)
    {
        player.GetPlayerShooting.AddWeapon(this);
    }

    public string GetInteractionText()
    {
        return $"Press E to pick up {WeaponName}";
    }

    public void SetZombieLayer(LayerMask layer)
    {
        zombieLayer = layer;
    }

    //Start shooting on mouse button down
    public virtual void StartShoot()
    {
        StopShoot();
        criticalShot = Random.Range(0, 101) <= CriticalChance;
        StartCoroutine(nameof(Shoot));
    }

    protected virtual IEnumerator Shoot()
    {
        yield return null;
    }

    //Stop shooting on mouse button up
    public virtual void StopShoot()
    {
        StopCoroutine(nameof(Shoot));
    }

    //Stop shooting and deactivate the weapon
    public void DeactivateWeapon()
    {
        StopCoroutine(nameof(StopShoot));
        gameObject.SetActive(false);
    }

#if UNITY_EDITOR

    //Make sure the values aren't negative
    protected virtual void OnValidate()
    {
        WeaponRange = WeaponRange <= 0 ? 0 : WeaponRange;
        WeaponDamage = WeaponDamage <= 0 ? 0 : WeaponDamage;
        CriticalMulitiplier = CriticalMulitiplier <= 0 ? 0 : CriticalMulitiplier;
    }

#endif

}

public enum WeaponType
{
    AutomaticShotgun, BurstRifle, AutomaticRifle, Pistol, Shotgun , Melee
}