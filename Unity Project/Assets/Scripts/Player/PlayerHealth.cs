using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    int tierRange; //How many value are in one tier
    int CurrentTier => Mathf.CeilToInt((float)currentHealth / tierRange); //Get which tier the player is in
    int currentTier;

    protected override void Start()
    {
        base.Start();
        SetID(PoolID.Player);
        tierRange = MaxHealth / 5; //5 is the number of health bars
        currentTier = CurrentTier; //Set the current tier
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
        UpdateHealthBar();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        UpdateHealthBar();

        //Turn off characters and stop coroutines
        if(currentHealth <= 0)
        {
            EventManager.playerDied.Invoke();
            EventManager.toggleCharacters(0);
        }
    }


    void UpdateHealthBar()
    {
        //When player change tier update current tier and UI health bar
        if(currentTier != CurrentTier)
        {
            currentTier = CurrentTier;
            UIManager.Instance.UpdateHealthBar(currentTier);
        }
    }
}