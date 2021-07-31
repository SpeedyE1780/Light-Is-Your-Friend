using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Range(10 , 200)]
    public int MaxHealth = 100;
    protected int currentHealth;

    PoolID poolID = PoolID.Zombie; // default value

    protected virtual void Start()
    {
        currentHealth = MaxHealth; //Set current health to max health
    }

    public void SetID(PoolID id)
    {
        poolID = id;
    }

    public virtual void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, MaxHealth); //Keep value between 0 and max
    }

    public virtual void TakeDamage(int damage)
    {
        Debug.Log($"Applied {damage} damage to {gameObject.name}");
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, MaxHealth); //Keep value between 0 and max

        //Destroy gameobject
        if (currentHealth <= 0)
        {
            PoolManager.Instance.AddToPool(poolID, gameObject);
            
            //Play zombie died sound
            if (poolID == PoolID.Zombie)
                SoundManager.Instance.ZombieDie.Play();
        }
    }
}